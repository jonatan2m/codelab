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
├── 02-long-polling/        ← experimento 2
├── 03-sse/                 ← experimento 3
└── 04-websocket/           ← experimento 4 (com Redis pub/sub)
```

Cada experimento tem seu próprio `docker-compose.yml` e instruções de execução.
