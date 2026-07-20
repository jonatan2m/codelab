import http from "http";
import os from "os";

const SERVER_HOST = "server";
const SERVER_PORT = 3000;
const CLIENT_ID = os.hostname();

// Node não tem o EventSource do browser embutido — este cliente reimplementa
// manualmente o que o EventSource faz por baixo dos panos: parse do protocolo
// SSE, rastreio do Last-Event-ID e reconexão automática.
let lastEventId = 0;
let reconnectDelayMs = 2_000; // sobrescrito pelo campo "retry:" do servidor

function connect(): void {
  const headers: http.OutgoingHttpHeaders = { Accept: "text/event-stream" };
  if (lastEventId > 0) headers["Last-Event-ID"] = String(lastEventId);

  console.log(
    `[CLIENT ${CLIENT_ID}] conectando em /events` +
    (lastEventId > 0 ? ` (Last-Event-ID=${lastEventId})` : "") +
    "..."
  );

  const req = http.request(
    { hostname: SERVER_HOST, port: SERVER_PORT, path: "/events", method: "GET", headers },
    (res) => {
      if (res.statusCode !== 200) {
        console.error(`[CLIENT ${CLIENT_ID}] status inesperado: ${res.statusCode}`);
        res.resume();
        scheduleReconnect();
        return;
      }

      let buffer = "";
      res.setEncoding("utf8");

      // mensagens SSE são separadas por linha em branco (\n\n)
      res.on("data", (chunk: string) => {
        buffer += chunk;
        let separatorIndex: number;
        while ((separatorIndex = buffer.indexOf("\n\n")) !== -1) {
          const rawMessage = buffer.slice(0, separatorIndex);
          buffer = buffer.slice(separatorIndex + 2);
          handleMessage(rawMessage);
        }
      });

      res.on("end", () => {
        console.log(`[CLIENT ${CLIENT_ID}] conexão encerrada pelo servidor — reconectando`);
        scheduleReconnect();
      });
    }
  );

  req.on("error", (err: Error) => {
    console.error(`[CLIENT ${CLIENT_ID}] erro na conexão: ${err.message} — reconectando`);
    scheduleReconnect();
  });

  req.end();
}

function handleMessage(rawMessage: string): void {
  let eventType = "message";
  let data = "";
  let id: number | undefined;

  for (const line of rawMessage.split("\n")) {
    if (line.startsWith(":")) continue; // heartbeat/comentário, sem efeito
    if (line.startsWith("retry:")) {
      reconnectDelayMs = Number(line.slice("retry:".length).trim());
      console.log(`[CLIENT ${CLIENT_ID}] servidor definiu retry=${reconnectDelayMs}ms`);
      continue;
    }
    if (line.startsWith("id:")) {
      id = Number(line.slice("id:".length).trim());
      continue;
    }
    if (line.startsWith("event:")) {
      eventType = line.slice("event:".length).trim();
      continue;
    }
    if (line.startsWith("data:")) {
      data += line.slice("data:".length).trim();
    }
  }

  if (!data) return; // era só retry/heartbeat, sem payload

  // é exatamente isso que o EventSource do browser faz sozinho: guarda o
  // último id visto pra mandar como header Last-Event-ID na próxima reconexão
  if (id !== undefined) lastEventId = id;

  const parsed = JSON.parse(data);
  console.log(`[CLIENT ${CLIENT_ID}] evento recebido (id=${id}, event=${eventType}):`, parsed);
}

function scheduleReconnect(): void {
  setTimeout(connect, reconnectDelayMs);
}

// canal reverso: cliente → servidor não usa a conexão SSE (que é só
// servidor → cliente), e sim uma requisição HTTP comum e separada
function sendReverseMessage(): void {
  const payload = JSON.stringify({
    from: CLIENT_ID,
    text: "ping do cliente",
    at: new Date().toISOString(),
  });

  const req = http.request(
    {
      hostname: SERVER_HOST,
      port: SERVER_PORT,
      path: "/messages",
      method: "POST",
      headers: { "Content-Type": "application/json", "Content-Length": Buffer.byteLength(payload) },
    },
    (res) => {
      res.resume();
      console.log(`[CLIENT ${CLIENT_ID}] mensagem enviada via POST /messages → status ${res.statusCode}`);
    }
  );

  req.on("error", (err: Error) => {
    console.error(`[CLIENT ${CLIENT_ID}] erro ao enviar mensagem: ${err.message}`);
  });

  req.end(payload);

  setTimeout(sendReverseMessage, 7_000 + Math.random() * 3_000);
}

// aguarda o servidor subir
setTimeout(() => {
  console.log(`[CLIENT ${CLIENT_ID}] iniciando cliente SSE\n`);
  connect();
  setTimeout(sendReverseMessage, 4_000);
}, 2_000);
