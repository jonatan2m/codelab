# Realtime Updates — Polling, Long Polling, SSE e WebSockets

Estudo prático dos quatro mecanismos de comunicação em tempo real, com foco em tradeoffs e simulações locais via Docker.

---

## 1. Polling

### O que é
O cliente faz requisições HTTP periódicas ao servidor para verificar se há dados novos. O servidor responde imediatamente, com ou sem novidades.

```
Client          Server
  |---GET /updates-->|
  |<--200 (vazio)----|   ← nada novo
  |  (aguarda 5s)    |
  |---GET /updates-->|
  |<--200 (dados)----|   ← agora tem
```

### Tradeoffs

| Prós | Contras |
|------|---------|
| Simples de implementar | Alto volume de requisições desnecessárias |
| Stateless no servidor | Latência alta (depende do intervalo) |
| Funciona em qualquer infraestrutura | Desperdício de banda e CPU |
| Fácil de debugar | Não escala bem com muitos clientes |

### Quando usar
- Dados que mudam raramente e latência não é crítica
- Ambientes onde SSE/WebSocket não são suportados
- Sistemas legados com restrições de infraestrutura

---

## 2. Long Polling

### O que é
O cliente faz uma requisição HTTP, mas o servidor **segura a conexão aberta** até ter dados novos para enviar (ou até um timeout). Quando responde, o cliente imediatamente faz outra requisição.

```
Client          Server
  |---GET /updates-->|
  |                  |  ← servidor aguarda dados...
  |                  |  ← (30s depois, chega um evento)
  |<--200 (dados)----|
  |---GET /updates-->|  ← cliente refaz imediatamente
  |                  |
```

### Tradeoffs

| Prós | Contras |
|------|---------|
| Latência menor que polling | Servidor precisa manter threads/conexões abertas |
| Menos requisições desnecessárias | Overhead de reconexão a cada mensagem |
| Compatível com proxies/firewalls | Timeouts precisam de tratamento cuidadoso |
| Sem infraestrutura especial | Não é eficiente para alta frequência de eventos |

### Como o servidor "segura" a resposta

O que segura a conexão é simples: **`res.end()` nunca foi chamado.**

Em Node, enquanto você não chamar `res.end()`, a conexão TCP permanece aberta e o cliente fica esperando. O servidor não faz nada ativo para "segurar" — ele simplesmente não responde. O `res` é um objeto JavaScript em memória apontando para o socket TCP aberto.

```
requisição chega → callback executa
pendingClients.push(res) → salva referência em memória
callback termina — mas res.end() NUNCA foi chamado
↓
conexão TCP continua aberta, cliente continua esperando
↓
Event Loop livre para processar outras requisições
```

O `setTimeout` de timeout **não segura** a conexão — ele faz o oposto: garante que a conexão seja encerrada caso nenhum evento apareça em N segundos.

Existem exatamente três caminhos que chamam `res.end()`:

| Situação | Quem encerra | Status |
|----------|-------------|--------|
| Evento chegou | `onEventArrived` | 200 |
| N segundos sem evento | `setTimeout` | 204 |
| Cliente desconectou | nenhum — só limpa a fila | — |

O servidor guarda a referência do objeto `res` (Node) ou usa `TaskCompletionSource` (C#) em memória. A thread/event loop fica livre para atender outras requisições. Quando um evento chega, o servidor usa essa referência para responder.

Três situações precisam ser tratadas:
- **Evento chega** → responde e remove da fila
- **Timeout** → retorna 204 No Content e remove da fila (cliente reconecta)
- **Cliente desconecta** → limpa a fila para não escrever num socket morto

```typescript
// Node.js — fila de clientes esperando
const pendingClients: http.ServerResponse[] = []

server.on("request", (req, res) => {
  pendingClients.push(res)

  // evento "close" é o equivalente Node do CancellationToken do C#:
  // disparado automaticamente quando o cliente fecha a conexão
  req.on("close", () => {
    const index = pendingClients.indexOf(res)
    if (index !== -1) pendingClients.splice(index, 1)
  })

  // timeout: nenhum evento em 30s → 204, cliente reconecta
  setTimeout(() => {
    const index = pendingClients.indexOf(res)
    if (index !== -1) {
      pendingClients.splice(index, 1)
      res.writeHead(204)
      res.end()
    }
  }, 30_000)
})

function onEventArrived(event: Event) {
  for (const res of pendingClients) {
    res.end(JSON.stringify(event))
  }
  pendingClients.length = 0
}
```

> **Node `req.on("close")` ↔ C# `CancellationToken`**
>
> São o mesmo conceito em linguagens diferentes: um sinal emitido automaticamente pelo runtime quando o cliente desconecta, permitindo que o servidor aborte o trabalho pendente e libere recursos. Em C#, o ASP.NET injeta o `CancellationToken` no método do controller e o cancela quando a conexão cai. Em Node, o evento `close` é emitido na `IncomingMessage` (`req`). Sem tratar esse sinal, o servidor acumula entradas órfãs na fila indefinidamente.

```csharp
// C# — TaskCompletionSource libera a thread enquanto espera
// ConcurrentDictionary é obrigatório: múltiplas threads do pool acessam simultaneamente
private static ConcurrentDictionary<string, TaskCompletionSource<Event>> _pending = new();

[HttpGet("/poll")]
public async Task<IActionResult> LongPoll(string userId, CancellationToken ct)
{
    var tcs = new TaskCompletionSource<Event>();
    _pending[userId] = tcs;

    // CancellationToken detecta se o cliente caiu e aborta a espera
    ct.Register(() => tcs.TrySetCanceled());

    try
    {
        var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(30_000, ct));
        _pending.TryRemove(userId, out _);

        if (completedTask == tcs.Task)
            return Ok(await tcs.Task);  // chegou evento
        else
            return NoContent();         // 204: timeout esperado, cliente reconecta
    }
    catch (OperationCanceledException)
    {
        _pending.TryRemove(userId, out _);
        return StatusCode(499);         // cliente desconectou
    }
}

public void NotifyClient(string userId, Event ev)
{
    if (_pending.TryGetValue(userId, out var tcs))
        tcs.TrySetResult(ev);           // TrySetResult: seguro se já foi cancelado
}
```

### Modelo thread-per-request vs async I/O

O problema de escala não é de linguagem — é de **modelo de execução**. C# moderno (ASP.NET Core + Kestrel) lida com conexões concorrentes tão bem quanto Node.js, pois também usa async/await com I/O não-bloqueante. A comparação correta é:

| Modelo | Exemplos | Comportamento com 10k conexões |
|--------|----------|-------------------------------|
| Thread-per-request bloqueante | PHP-FPM, Java Servlet clássico, IIS classic pipeline | ~10GB RAM + context switching + risco de port exhaustion |
| Async I/O | Node.js, ASP.NET Core, Go, Java Netty | Kilobytes por conexão, thread liberada enquanto aguarda |

**Port exhaustion**: cada conexão TCP ativa precisa de uma porta efêmera no SO. O limite real fica em torno de 28k–60k conexões simultâneas dependendo do SO e configuração — muito menos que os 65k teóricos.

**Node.js especificamente**: usa uma única thread com Event Loop. O objeto `res` fica em memória como referência JavaScript (kilobytes). O Event Loop continua processando outras requisições enquanto aguarda o evento que vai resolver a resposta pendente.

### Onde long polling é/foi usado na prática

- **Pagamentos** — cliente faz POST, recebe jobId, abre long poll em `/payment/{id}/status`. PSP manda webhook → servidor resolve → cliente recebe confirmação
- **Facebook Chat (2008), Gmail (versão antiga)** — antes do WebSocket, cada aba aberta = uma conexão long poll esperando mensagens
- **CI/CD (logs de build)** — frontend segura conexão esperando próximo log; build runner publica evento quando step conclui
- **Jira/Confluence** — notificações de atualização de tickets

### A cadeia precisa ser event-driven

Long polling no servidor não resolve nada se ele mesmo fizer polling internamente. Alguém na cadeia precisa notificar via evento:
- Webhook do serviço externo
- Mensagem numa fila (Kafka, RabbitMQ, Redis Pub/Sub)
- Evento de banco de dados (PostgreSQL `LISTEN/NOTIFY`)

### Problema clássico: "thundering herd"
Se o servidor reinicia ou um evento chega para muitos clientes simultaneamente, todos reconectam ao mesmo tempo, sobrecarregando o servidor.

Não é carga constante — é um **pico sincronizado**. No nosso código, isso acontece porque `onEventArrived` itera todos os `pendingClients` e responde todo mundo no mesmo laço (`02-long-polling/server/src/index.ts`). Cada um desses clientes, ao receber a resposta, reconecta imediatamente (`setTimeout(poll, 0)` no cliente) — e todas essas reconexões chegam ao servidor praticamente no mesmo milissegundo.

**Reproduzindo na prática:** o `02-long-polling` sobe **15 réplicas do client** (`deploy.replicas` no `docker-compose.yml`) e o servidor imprime um histograma (`[THUNDERING HERD]`) mostrando em qual janela de 100ms, após cada broadcast, as reconexões chegaram:

```bash
cd 02-long-polling
docker compose up --build
```

Sem mitigação, o histograma mostra tudo empilhado em `+0ms`:
```
[THUNDERING HERD] Reconexões recebidas nos 3s após o broadcast para 15 cliente(s):
  +   0ms | ███████████████ 15
```

**Mitigação: jitter.** Em vez de reconectar imediatamente, cada cliente espera um valor aleatório entre `0` e `JITTER_MS` antes de refazer a requisição (`Math.random() * JITTER_MS`). Isso espalha as reconexões ao longo de uma janela, em vez de todas caírem no mesmo instante:

```bash
JITTER_MS=800 docker compose up --build
```

```
[THUNDERING HERD] Reconexões recebidas nos 3s após o broadcast para 15 cliente(s):
  +   0ms | ██ 2
  + 100ms | ███ 3
  + 200ms | ███ 3
  + 300ms | █ 1
  + 500ms | █ 1
  + 600ms | ██ 2
  + 800ms | █ 1
```

O total de reconexões é o mesmo — o que muda é a distribuição no tempo, evitando o pico. Essa é a mesma ideia por trás de "exponential backoff with jitter", usada em qualquer cliente que precisa retentar contra um servidor compartilhado (SDKs de cloud, filas, etc).

---

## 3. Server-Sent Events (SSE)

### O que é
O servidor mantém uma **conexão HTTP unidirecional** aberta e empurra eventos para o cliente quando quiser. O cliente usa a API `EventSource` (browser) ou HTTP streaming.

```
Client          Server
  |---GET /events--->|
  |<--data: msg1-----|
  |<--data: msg2-----|   ← servidor empurra quando quiser
  |<--data: msg3-----|
  |                  |   ← conexão permanece aberta
```

### Formato do protocolo
```
data: {"message": "hello"}\n\n
event: user_joined\n
data: {"user": "alice"}\n\n
id: 42\n
data: {"tick": true}\n\n
```

### Tradeoffs

| Prós | Contras |
|------|---------|
| Reconnect automático (built-in) | Unidirecional (cliente → servidor precisa de outro canal) |
| Leve e baseado em HTTP/1.1 | Limite de 6 conexões por domínio (HTTP/1.1) |
| IDs de evento permitem retomada | Não suportado nativamente em alguns ambientes |
| Multiplexado no HTTP/2 | Não é adequado para comunicação bidirecional |

### Caso de uso ideal
- Feeds de notificações
- Dashboards com métricas em tempo real
- Progresso de operações longas (CI/CD, uploads)
- Streaming de respostas de LLM/chat (ver `bonus-sse-chatbot` e `bonus-sse-chatbot-jobs` mais abaixo)

### Como o servidor mantém a conexão aberta (e nunca chama `res.end()`)

Igual ao long polling, quem "segura" a conexão é simplesmente **nunca chamar `res.end()`** enquanto o cliente estiver conectado. A diferença é o que acontece nesse meio tempo: no long polling, o servidor guarda o `res` esperando **um único** evento e depois encerra (`res.end()`); no SSE, o servidor chama `res.write()` **repetidamente**, quantas vezes quiser, e só remove o cliente da lista quando ele desconecta.

```typescript
// Node.js — a cada evento novo, escreve em todos os clientes conectados sem nunca encerrar
function broadcast(event: ServerEvent): void {
  for (const res of connectedClients) {
    res.write(`id: ${event.id}\n`);
    res.write(`data: ${JSON.stringify(event)}\n\n`);
  }
}
```

| | Long Polling | SSE |
|--|--------------|-----|
| Quantos eventos por conexão | 1 (depois `res.end()`) | Vários (`res.write()` repetido) |
| Quem fecha a conexão | Servidor, a cada evento/timeout | Cliente desconecta, ou servidor cai |
| Reconexão | Cliente reabre a cada resposta | Só quando a conexão cai de fato |

### `retry:` e heartbeat

Duas linhas do protocolo controlam o comportamento de conexão, não o dado em si:

- **`retry: 2000\n\n`** — diz ao `EventSource` (ou a qualquer cliente que implemente o protocolo) quantos ms esperar antes de reconectar automaticamente se a conexão cair. Sem isso, o cliente usa um default (~3s no browser).
- **`:\n\n`** (linha começando com `:`) — é um **comentário SSE**, ignorado pelo parser. Serve só como heartbeat: mantém a conexão "viva" aos olhos de proxies/load balancers que derrubam conexões ociosas por timeout. No `03-sse`, o servidor manda um a cada 15s.

### Last-Event-ID — como funciona a retomada

Cada evento carrega um `id:`. O `EventSource` do browser guarda automaticamente o último `id` recebido; se a conexão cair, ele reconecta sozinho e manda esse valor de volta como **header** `Last-Event-ID`. Cabe ao servidor decidir o que fazer com isso — nada no protocolo obriga a reenviar o que foi perdido, é uma convenção que o servidor precisa implementar.

```
Client                          Server
  |---GET /events--------------->|
  |<--id:1 data:{...}------------|
  |<--id:2 data:{...}------------|
  |          (conexão cai)       |
  |---GET /events----------------|
  |    Last-Event-ID: 2          |
  |<--id:3 data:{...}------------|   ← só o que faltou, não o histórico inteiro
  |<--id:4 data:{...}------------|
```

No `03-sse`, isso vira um **event log em memória** (`eventLog: ServerEvent[]`) no servidor. Quando alguém conecta com `Last-Event-ID`, o servidor filtra `eventLog.filter(e => e.id > lastEventId)` e reenvia só o que faltou, antes de voltar a transmitir eventos novos ao vivo:

```typescript
const lastEventId = Number(req.headers["last-event-id"]) || 0;

if (lastEventId > 0) {
  const missed = eventLog.filter((e) => e.id > lastEventId);
  for (const event of missed) writeEvent(res, event);
}

connectedClients.push(res);
```

> Isso é uma simplificação didática: o log vive na memória do processo, então um restart do servidor apaga o histórico. Em produção isso seria persistido (Redis, um tópico Kafka com retenção, etc.), do mesmo jeito que o pub/sub do WebSocket (seção 4) também precisa de algo externo pra coordenar múltiplas instâncias.

**Node não tem `EventSource` embutido** (é uma API de browser). Por isso o cliente do `03-sse` reimplementa manualmente o parser do protocolo: separa mensagens por `\n\n`, interpreta as linhas `id:`/`event:`/`data:`/`retry:`, guarda o último id visto e reconecta sozinho — exatamente o que o browser faz por trás da API `new EventSource(url)`.

### Canal reverso: cliente → servidor

SSE é **só** a direção servidor → cliente. Não existe "escrever de volta" na mesma conexão — ela é HTTP normal, só que a resposta nunca termina. Pra mandar dados na direção contrária, o cliente faz uma requisição HTTP separada e independente (`POST`), enquanto a conexão SSE continua aberta só recebendo:

```
EventSource (GET /events)  ──────► fica aberto, só recebe
fetch/POST (/messages)     ──────► request comum, isolado da conexão acima
```

No `03-sse`, o cliente manda `POST /messages` a cada ~7-10s pra simular esse padrão (é assim, por exemplo, que um chat com SSE funciona: SSE entrega mensagens novas, um POST comum manda a mensagem que você digitou).

### C# — implementação equivalente

Sem SignalR: `Response.WriteAsync()` em loop, com `FlushAsync()` depois de cada evento (sem isso, o Kestrel pode segurar o dado em buffer e o cliente não recebe nada até o buffer encher):

```csharp
[HttpGet("/events")]
public async Task GetEvents(CancellationToken ct)
{
    Response.Headers.Add("Content-Type", "text/event-stream");
    Response.Headers.Add("Cache-Control", "no-cache");
    await Response.WriteAsync($"retry: 2000\n\n", ct);

    var lastEventId = int.TryParse(Request.Headers["Last-Event-ID"], out var id) ? id : 0;
    if (lastEventId > 0)
    {
        foreach (var missed in _eventLog.Where(e => e.Id > lastEventId))
            await WriteEvent(missed, ct);
    }

    var channel = _hub.Subscribe();   // Channel<ServerEvent> — igual ao connectedClients.push(res) do Node
    try
    {
        await foreach (var ev in channel.Reader.ReadAllAsync(ct))
            await WriteEvent(ev, ct);
    }
    finally
    {
        _hub.Unsubscribe(channel);    // equivalente ao req.on("close") do Node
    }
}

private async Task WriteEvent(ServerEvent ev, CancellationToken ct)
{
    await Response.WriteAsync($"id: {ev.Id}\ndata: {JsonSerializer.Serialize(ev)}\n\n", ct);
    await Response.Body.FlushAsync(ct);
}
```

A diferença estrutural pro Node: em C# não existe uma lista `http.ServerResponse[]` global tão natural — o idiomático é um `Channel<T>` (fila assíncrona) por conexão, com um `Subscribe/Unsubscribe` central, porque múltiplas threads do pool podem estar publicando eventos ao mesmo tempo. O `CancellationToken` (`ct`) faz o mesmo papel do `req.on("close")` do Node: sinaliza quando o cliente caiu pra sair do loop e liberar recursos.

### Rodando o experimento

```bash
cd 03-sse
docker compose up --build
```

Sobe 1 servidor + 3 réplicas de cliente. Pra ver o replay de `Last-Event-ID` na prática sem depender de timing de rede do Docker, dá pra simular uma reconexão direto com `curl` enquanto o compose estiver rodando:

```bash
curl -N http://localhost:3002/events -H "Last-Event-ID: 5"
```

Isso retoma a partir do evento 6 imediatamente — os eventos perdidos chegam todos de uma vez, antes de voltar a receber eventos novos ao vivo.

**Cliente browser de verdade:** com o compose rodando, abra `http://localhost:3002/` no navegador. É uma página estática servida pelo próprio servidor Node (`GET /`), usando `EventSource` nativo em vez do parser manual que o cliente Node usa — dá pra comparar os dois lado a lado. A página mostra o status da conexão, o `Last-Event-ID` sendo atualizado sozinho pelo browser, e tem um campo pra testar o canal reverso (`POST /messages`). Pra ver a reconexão automática ao vivo: DevTools → aba Network → marca "Offline" por uns segundos → desmarca. Nenhuma linha de código nossa faz isso — é o `EventSource` cuidando de tudo sozinho.

> Dá pra usar essa mesma página pra sentir o limite de 6 conexões por domínio do HTTP/1.1 na prática: abra 7+ abas em `http://localhost:3002/` e observe, na aba Network da 7ª, a conexão SSE ficando "pendente" até uma das outras 6 fechar.

### Caso de uso real: chatbot com streaming de LLM (`bonus-sse-chatbot`)

É literalmente assim que a API da Anthropic/OpenAI streama respostas de chat hoje. Vale entender por que SSE encaixa bem aqui e como isso escala — é um padrão de uso **diferente** do `03-sse` acima, e essa diferença importa:

| | `03-sse` (feed de notificações) | `bonus-sse-chatbot` (streaming de LLM) |
|--|-----------------------------------|------------------------------------------|
| Conexão | `GET /events`, de longa duração, existe **antes** de haver dado pra mandar | `POST /chat`, dura só o tempo de **uma** resposta |
| Quem inicia o stream | Servidor decide quando falar (evento pode nunca vir) | Stream começa porque o cliente pediu algo específico |
| Reconexão / `Last-Event-ID` | Faz sentido — o cliente pode ter perdido eventos enquanto estava fora | Não se aplica — se cair no meio, a prática comum é só refazer a pergunta |
| Múltiplos clientes recebendo o mesmo evento | Sim (broadcast) | Não — cada resposta é 1:1 com quem perguntou |

Ou seja: SSE não é só "um feed que fica ligado pra sempre" — pode ser simplesmente **a resposta de uma única requisição, indo em pedaços**, o que é exatamente o que acontece quando você chama a API de chat de um LLM e lê a resposta como stream.

**Por que não precisa de pub/sub pra escalar:** ao contrário do WebSocket com broadcast (seção 4, onde um evento em um servidor precisa alcançar clientes conectados em *outros* servidores), aqui cada stream pertence a exatamente um usuário. Não há fan-out entre instâncias — qualquer servidor atrás do load balancer resolve a conversa sozinho. O gargalo real de "milhares de usuários simultâneos" não é a camada de transporte (I/O assíncrono aguenta milhares de conexões abertas, como já vimos), é **quantas inferências de LLM concorrentes o seu backend consegue sustentar** — isso é limite/fila do provedor do modelo, não do protocolo HTTP.

**Pegadinha de infra:** reverse proxies (Nginx, muitos load balancers) fazem *buffering* de resposta por padrão — esperam o corpo inteiro antes de repassar ao cliente, o que quebra streaming (usuário só veria a resposta pronta, de uma vez, no final). Em produção isso exige desabilitar buffering explicitamente para a rota de streaming (`proxy_buffering off` no Nginx, ou equivalente no seu gateway).

**Gotcha real do Node encontrado montando esse experimento:** o servidor precisa ler o corpo da requisição (`POST` com o JSON da pergunta) antes de começar a streamar a resposta. Uma vez que esse corpo é totalmente consumido (`req.on("data")`/`req.on("end")`), o Node emite `close` no objeto `req` sozinho — **mesmo com o socket TCP ainda vivo**, streamando a resposta. Se você usar `req.on("close")` pra detectar desconexão aqui (como fizemos no `02-long-polling`, onde funciona porque o `GET` nunca tem corpo lido), ele dispara segundos cedo demais e a conversa é marcada como "abortada" logo ao começar. O sinal confiável é o `close` da **resposta**, checando se ela já tinha terminado normalmente:

```typescript
// ❌ dispara logo após o body da requisição ser consumido, não quando o cliente cai de verdade
req.on("close", () => { /* falso positivo */ });

// ✅ só considera desconexão real se a resposta ainda não tinha terminado
res.on("close", () => {
  if (!res.writableEnded) {
    // cliente realmente caiu no meio do streaming
  }
});
```

**Rodando o experimento:**

```bash
cd bonus-sse-chatbot
docker compose up --build
```

Sobe 1 servidor + 5 réplicas de cliente, cada uma simulando um usuário fazendo perguntas em intervalos aleatórios (o "ritmo de conversa" mencionado no início). No log do servidor dá pra ver `streams ativos` subindo até 5 com conversas concorrentes intercaladas — e no log de cada cliente (`docker logs bonus-sse-chatbot-client-N`), só os tokens da própria conversa aparecem, mesmo com tudo rodando num único processo Node single-threaded.

### Indo além: desacoplando geração de entrega (`bonus-sse-chatbot-jobs`)

O `bonus-sse-chatbot` acima tem uma limitação de design: a geração da resposta acontece **dentro** do handler da requisição SSE. Se o cliente desconectar no meio (fechou a aba, app crashou, rede caiu), a geração para junto — o trabalho (e o custo de inferência, se fosse um LLM de verdade) é perdido, e o cliente não tem como recuperar a resposta sem pagar/gerar tudo de novo.

Empresas que vendem isso como serviço (é assim que APIs de chat de LLM em produção funcionam) resolvem isso desacoplando **quem gera** de **quem está ouvindo**:

```
POST /jobs              → cria o job, geração começa AGORA em segundo plano,
                           responde na hora com { jobId } (não é streaming)

GET /jobs/{id}/stream   → só "assiste" a geração em andamento (ou já concluída).
                           Cai? Reconecta no mesmo jobId depois — sem perder nada.
```

A ideia central: a geração vive presa ao **job**, guardada num array em memória (`job.tokens`) no servidor, não presa a nenhuma conexão. Quando alguém conecta (ou reconecta) em `/jobs/{id}/stream`, o servidor primeiro faz um **catch-up** — reenvia tudo que já foi gerado até ali — e só depois volta a mandar tokens novos ao vivo:

```typescript
// catch-up: manda tudo que já foi gerado antes desse cliente conectar/reconectar
for (const token of job.tokens) {
  res.write(`data: ${JSON.stringify({ token })}\n\n`);
}

if (job.status === "done") {
  res.write(`event: done\ndata: {}\n\n`);
  res.end();
  return;
}

job.subscribers.add(res); // só a partir daqui esse cliente recebe tokens novos ao vivo
```

E a geração roda num laço independente, sem checar se tem alguém ouvindo:

```typescript
function step(): void {
  if (index >= words.length) { /* ...marca job.status = "done", encerra assinantes... */ }

  const token = words[index];
  job.tokens.push(token);              // sobrevive a qualquer desconexão
  broadcastToJob(job, `data: ${JSON.stringify({ token })}\n\n`); // só quem estiver ouvindo recebe agora
  index++;
  setTimeout(step, 50 + Math.random() * 150);
}
```

Isso é a mesma ideia do `Last-Event-ID` do `03-sse` (retomar sem perder o que já aconteceu), só que aplicada no nível do **job** em vez de um header por evento — mais simples de implementar e mais alinhado com "essa resposta é cara de gerar, não pode se perder".

**Rodando o experimento:**

```bash
cd bonus-sse-chatbot-jobs
docker compose up --build
```

Metade das conversas dos clientes simula uma queda de conexão de propósito (após receber 3 tokens, chama `req.destroy()`), espera um pouco, e reconecta no mesmo `jobId`. No log do servidor (`docker compose logs -f server`) procure por `CONTINUA em segundo plano` (a geração não parou) seguido de `replaying N token(s) já gerado(s)` (a reconexão pegou o histórico certinho) — é a prova de que geração e entrega são, de fato, independentes uma da outra.

### Limitações e escala real: SSE dá conta de milhões de usuários?

Resumo direto: **a conexão em si, sim. O que os nossos experimentos fazem por trás dela, não — de propósito.** Os experimentos guardam tudo em memória (`eventLog`, `jobs`) porque são didáticos; uma versão de produção precisa resolver os pontos abaixo.

**1. Uma instância tem teto, sempre.** Async I/O aguenta dezenas de milhares de conexões abertas por processo (kilobytes cada, não é isso que limita) — mas existe teto real de file descriptors (`ulimit -n`) e de portas efêmeras do SO (~28k–60k por IP, como vimos na seção de long polling). "Milhões de usuários simultâneos" nunca cabe numa máquina só. Sempre significa **múltiplas instâncias atrás de um load balancer**.

**2. Broadcast entre instâncias — o mesmo problema do WebSocket, sem exceção pro SSE.** O README dá a entender que "sticky sessions"/pub-sub é problema só de WebSocket (seção 4) — não é. Vale reforçar: se o seu SSE faz **broadcast** (como o `03-sse`, onde um evento deve chegar a todo mundo conectado), e você tem 5 instâncias atrás de um load balancer, um evento gerado na instância A **não chega sozinho** aos clientes conectados nas instâncias B, C, D, E. Precisa do mesmo Redis Pub/Sub (ou Kafka) da seção 4 pra propagar entre instâncias. A diferença real não é "SSE escapa disso" — é que **o chatbot** (`bonus-sse-chatbot*`) escapa disso, porque cada stream é 1:1 e não broadcast. Escala sem pub/sub é uma propriedade do *caso de uso* (broadcast vs. 1:1), não do protocolo escolhido.

**3. Estado em memória não sobrevive a múltiplas instâncias nem a restarts.** No `03-sse`, se o cliente reconectar numa instância *diferente* da que recebeu o evento original, o `eventLog` dessa nova instância não tem o histórico — o replay do `Last-Event-ID` simplesmente não encontra nada pra reenviar. Mesma coisa pro `jobs: Map` do `bonus-sse-chatbot-jobs`. Pra escalar de verdade, esse estado precisa sair da memória do processo e virar algo compartilhado (Redis, um tópico com retenção, um banco) — acessível por **qualquer** instância, não só a que originou o dado.

**4. Deploys com milhões de conexões de longa duração exigem *connection draining*.** Reiniciar uma instância com 50 mil conexões SSE abertas, sem cuidado, derruba as 50 mil de uma vez — e todas tentam reconectar ao mesmo tempo (o mesmo thundering herd da seção 2, só que em escala de produção). A prática comum: o load balancer para de mandar conexões *novas* pra instância que vai cair, ela sinaliza os clientes conectados pra migrar (um evento customizado tipo `event: reconnect`, ou simplesmente deixa o timeout natural cuidar disso com jitter), e só então a instância desliga.

**5. Load balancers/proxies gerenciados têm timeout de conexão idle.** Muitos (ALB, Cloudflare, Nginx) fecham conexões ociosas depois de um tempo configurável — se for menor que o intervalo entre eventos, a conexão cai "sem motivo aparente". É por isso que existe heartbeat (seção acima): mantém a conexão ocupada aos olhos do proxy, mesmo sem dado real pra mandar.

**6. Quando SSE deixa de ser a resposta certa em escala massiva.** Fan-out pra milhões de conexões simultâneas (ex: placar de jogo ao vivo, todo mundo vendo o mesmo evento) é uma arquitetura válida com SSE + pub/sub, mas cara de operar você mesmo. Nesse ponto, a maioria das empresas usa um serviço gerenciado de real-time (Ably, Pusher, PubNub, AWS AppSync, Cloudflare) que já resolve fan-out, presença e reconexão em escala — construir e operar isso do zero só compensa se real-time for o produto principal da empresa, não um recurso dele.

**Resumindo em uma frase:** a conexão HTTP escala (é I/O assíncrono, barato); o que não escala de graça é o estado que vive só na memória de um processo — isso é engenharia de sistema distribuído, independente de você ter escolhido SSE ou WebSocket.

---

## 4. WebSockets

### O que é
Um **canal bidirecional full-duplex** sobre uma única conexão TCP persistente. Começa como HTTP (handshake de upgrade) e depois vira um protocolo próprio.

```
Client          Server
  |---HTTP Upgrade-->|
  |<--101 Switching--|   ← handshake
  |<===TCP canal====>|   ← bidirecional e persistente
  |--msg: "hello"--->|
  |<--msg: "world"---|
  |--msg: "ping"---->|
```

### Tradeoffs

| Prós | Contras |
|------|---------|
| Full-duplex com baixa latência | Stateful: servidor precisa gerenciar conexões |
| Ideal para alta frequência de mensagens | Mais complexo para escalar horizontalmente |
| Sem overhead de HTTP por mensagem | Proxies/firewalls podem bloquear |
| Suporte a binário e texto | Sem reconnect automático (precisa implementar) |

### Problema de escala: sticky sessions
Num cluster com múltiplos servidores, um cliente conectado ao servidor A não recebe mensagens publicadas para clientes no servidor B. Solução comum: **pub/sub centralizado** (Redis, Kafka) para propagar eventos entre instâncias.

```
            ┌─────────┐
Client A ───┤ Server1 ├──┐
            └─────────┘  │   ┌───────┐
                         ├───┤ Redis │  ← broadcast
            ┌─────────┐  │   └───────┘
Client B ───┤ Server2 ├──┘
            └─────────┘
```

### Implementação (`04-websocket`)

Diferente do SSE, onde parsear o protocolo de texto na mão tinha valor didático, o framing binário do WebSocket (máscaras, opcodes, fragmentação) não é algo que se escreve à mão em produção — nem nós fizemos aqui. O servidor usa a lib `ws` (a mais usada no ecossistema Node), do mesmo jeito que uma aplicação real faria:

```typescript
import { WebSocketServer, WebSocket } from "ws";

// o mesmo servidor HTTP recebe o handshake de upgrade — não existe uma
// porta separada pro WebSocket, é a mesma conexão TCP mudando de protocolo
const wss = new WebSocketServer({ server: httpServer });

const localClients = new Set<WebSocket>();

wss.on("connection", (ws) => {
  localClients.add(ws);

  ws.on("message", async (raw) => {
    const data = JSON.parse(raw.toString());
    // publica no Redis em vez de mandar direto pros localClients — é isso
    // que faz TODAS as instâncias tratarem a mensagem da mesma forma
    await publisher.publish(CHANNEL, JSON.stringify({ ...data, instance: INSTANCE_ID }));
  });

  ws.on("close", () => localClients.delete(ws));
});

// toda instância está inscrita nesse canal — inclusive a que originou a
// mensagem, por isso ela também "recebe de volta" a própria mensagem
await subscriber.subscribe(CHANNEL, (message) => {
  for (const client of localClients) {
    if (client.readyState === WebSocket.OPEN) client.send(message);
  }
});
```

O experimento sobe **2 instâncias reais** (`server-a`, `server-b`), cada uma em uma porta diferente, as duas conectadas ao mesmo Redis — exatamente o cenário de "cluster com múltiplos servidores" da seção acima, sem simular nada.

### Cliente browser (o uso real)

Ao contrário dos experimentos anteriores, aqui o cliente principal **é o HTML** — é assim que WebSocket é consumido na prática (chat, board colaborativo, jogo multiplayer):

```javascript
const socket = new WebSocket("ws://" + location.host);

socket.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(data.from, data.text);
};

socket.onclose = () => {
  // ao contrário do EventSource, isso NÃO acontece sozinho —
  // reconectar é responsabilidade da aplicação
  setTimeout(connect, reconnectDelay);
};

form.addEventListener("submit", (e) => {
  e.preventDefault();
  socket.send(JSON.stringify({ from: myName, text: input.value })); // mesmo canal, sem endpoint separado
});
```

Repare no contraste com o SSE: lá, mandar dado pro servidor exigia um `POST` separado (`fetch`) porque a conexão só recebia. Aqui, é `socket.send()` na mesma conexão que está recebendo — é isso que "full-duplex" significa na prática.

### C# — implementação equivalente

ASP.NET Core expõe isso via `HttpContext.WebSockets.AcceptWebSocketAsync()`, tipicamente dentro de um middleware:

```csharp
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        await HandleConnection(ws, context.RequestAborted);
    }
    else
    {
        await next();
    }
});

async Task HandleConnection(WebSocket ws, CancellationToken ct)
{
    _localClients.Add(ws);
    var buffer = new byte[4096];

    try
    {
        while (ws.State == WebSocketState.Open)
        {
            var result = await ws.ReceiveAsync(buffer, ct);
            if (result.MessageType == WebSocketMessageType.Close) break;

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            await _redisPublisher.PublishAsync(Channel, message); // mesmo padrão do Node
        }
    }
    finally
    {
        _localClients.Remove(ws); // equivalente ao ws.on("close") do Node
    }
}
```

A diferença estrutural: em C# não existe um evento `message`/`close` — é um **loop explícito** chamando `ReceiveAsync` até a conexão fechar. É mais verboso que o modelo orientado a eventos do Node, mas o conceito (guardar a conexão numa coleção local, publicar no Redis, repassar pra quem está inscrito) é idêntico.

### Rodando o experimento

```bash
cd 04-websocket
docker compose up --build
```

Sobe: Redis, `server-a` (porta 3005), `server-b` (porta 3006), e 2 bots conectados em cada instância gerando tráfego de fundo. Abra `http://localhost:3005/` numa aba e `http://localhost:3006/` em outra — mande uma mensagem em cada uma e observe:

- Mensagens da **própria instância** aparecem com rótulo verde
- Mensagens que chegaram **via Redis, originadas na outra instância** aparecem em amarelo

No log do servidor (`docker compose logs -f server-b`), procure por `recebido via Redis (originado na instância A)` — é a prova de que o broadcast está de fato atravessando instâncias, não é só um Set local.

---

## Apêndice TypeScript — Conceitos usados nos experimentos

### Union Types

Restringe uma variável a um conjunto fixo de valores. Equivalente a um enum de strings:

```typescript
type EventType = "new_order" | "payment_confirmed" | "user_signup"

const e: EventType = "new_order"     // ✅
const e: EventType = "invalid"       // ❌ erro de compilação
```

### Discriminated Union (Tagged Union)

Quando cada variante de um union carrega dados diferentes, você usa um campo discriminador (a "tag") para o TypeScript saber qual variante está sendo tratada:

```typescript
interface NewOrder {
  type: "new_order"       // ← tag
  value: number
}

interface PaymentConfirmed {
  type: "payment_confirmed"  // ← tag
  transactionId: string
}

type Event = NewOrder | PaymentConfirmed

function handle(event: Event) {
  switch (event.type) {
    case "new_order":
      console.log(event.value)          // TS sabe que é NewOrder aqui
      break
    case "payment_confirmed":
      console.log(event.transactionId)  // TS sabe que é PaymentConfirmed aqui
      break
  }
}
```

É o mesmo conceito de **tagged union** do Rust/Haskell/F#. A diferença é que nessas linguagens o compilador garante que todos os casos foram tratados (`match` exhaustivo). No TypeScript você consegue o mesmo efeito com:

```typescript
default:
  const _exhaustive: never = event // erro de compilação se algum caso faltar
```

### Interface vs Type

Para objetos simples, são equivalentes:

```typescript
interface User { id: number; name: string }
type User = { id: number; name: string }
```

Diferença prática: `interface` suporta declaration merging (pode ser estendida depois); `type` é mais flexível para unions e tipos compostos.

### Thread Safety e ConcurrentDictionary (C#)

**Por que `Dictionary` não é thread-safe**

Adicionar um item envolve múltiplos passos internos: calcular hash, achar bucket, verificar colisão, escrever valor, atualizar contador. Se duas threads fazem isso simultaneamente, uma pode sobrescrever o trabalho da outra no meio — dado corrompido ou loop infinito.

**Como `ConcurrentDictionary` resolve**

Usa duas estratégias:

*1. Lock por segmento (striped locking)*: em vez de um lock global, divide os buckets em segmentos com locks independentes. Threads que acessam segmentos diferentes rodam em paralelo sem se bloquear.

```
Bucket 0-15  → Lock A    ← Thread A escreve aqui
Bucket 16-31 → Lock B    ← Thread B escreve aqui (sem bloqueio mútuo)
Bucket 32-47 → Lock C
```

*2. Operações atômicas para leituras*: usa instruções de CPU como `Interlocked.CompareExchange` — indivisíveis por hardware, sem precisar de lock.

**O problema que ainda existe: check-then-act**

`ConcurrentDictionary` garante operações individuais, mas sequências de operações ainda podem ter race condition:

```csharp
// ❌ outra thread pode inserir entre o ContainsKey e o acesso
if (!_pending.ContainsKey(userId))
    _pending[userId] = new TaskCompletionSource<Event>();

// ✅ check + insert numa operação atômica só
_pending.GetOrAdd(userId, _ => new TaskCompletionSource<Event>());
```

Por isso `ConcurrentDictionary` expõe `GetOrAdd`, `AddOrUpdate`, `TryUpdate` — operações compostas que executam atomicamente.

**Node.js não tem esse problema**

Event Loop é single-threaded: nunca há duas execuções simultâneas acessando o mesmo objeto. Um `Map` simples já é "thread-safe" por design. A desvantagem é que código CPU-intensivo bloqueia todo mundo — por isso Node usa `worker_threads` para trabalho pesado.

### setInterval vs setTimeout em polling

```typescript
// ❌ setInterval — dispara a cada 1s independente da resposta anterior
setInterval(poll, 1000)

// ✅ setTimeout no callback — próxima requisição só sai após a resposta
function poll() {
  http.request(options, (res) => {
    res.on("end", () => setTimeout(poll, 1000)) // agenda após receber
  }).end()
}
```

Com `setInterval`, se uma requisição demorar mais que o intervalo, múltiplas ficam in-flight simultaneamente, causando acúmulo de requisições pendentes.

---

## Comparativo Geral

| Critério | Polling | Long Polling | SSE | WebSocket |
|----------|---------|--------------|-----|-----------|
| Direção | C→S | C→S | S→C | Bidirecional |
| Latência | Alta | Média | Baixa | Muito baixa |
| Overhead de rede | Alto | Médio | Baixo | Muito baixo |
| Complexidade server | Muito baixa | Baixa | Média | Alta |
| Reconexão automática | Manual | Manual | Nativa | Manual |
| Escala horizontal | Fácil | Fácil | Médio | Difícil (precisa pub/sub) |
| Suporte em proxies | Universal | Universal | Bom | Problemático |

---

## Estrutura dos experimentos

```
realtime-updates/
├── README.md               ← este arquivo
├── 01-polling/             ← experimento 1
├── 02-long-polling/        ← experimento 2 (+ demo de thundering herd)
├── 03-sse/                 ← experimento 3
├── bonus-sse-chatbot/      ← aplicação de SSE: streaming de chat estilo LLM
├── bonus-sse-chatbot-jobs/ ← mesma ideia, geração desacoplada da conexão (padrão job)
└── 04-websocket/           ← experimento 4: chat com Redis pub/sub, 2 instâncias + cliente HTML real
```

Cada experimento tem seu próprio `docker-compose.yml` e instruções de execução.
