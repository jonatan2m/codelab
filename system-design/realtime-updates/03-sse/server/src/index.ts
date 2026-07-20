import http from "http";
import fs from "fs";
import path from "path";

const indexHtml = fs.readFileSync(path.join(__dirname, "../public/index.html"), "utf-8");

type EventType = "new_order" | "payment_confirmed" | "user_signup";

interface ServerEvent {
  id: number;
  type: EventType;
  payload: { value: number };
  timestamp: string;
}

const EVENT_TYPES: EventType[] = ["new_order", "payment_confirmed", "user_signup"];
const HEARTBEAT_MS = 15_000;
const RETRY_MS = 2_000; // delay de reconexão que o EventSource deve usar
const MAX_LOG_SIZE = 100;

let nextEventId = 1;
let totalConnections = 0;

// histórico em memória — é o que permite responder ao Last-Event-ID
const eventLog: ServerEvent[] = [];
const connectedClients: http.ServerResponse[] = [];

function writeEvent(res: http.ServerResponse, event: ServerEvent): void {
  res.write(`id: ${event.id}\n`);
  res.write(`data: ${JSON.stringify(event)}\n\n`);
}

function broadcast(event: ServerEvent): void {
  eventLog.push(event);
  if (eventLog.length > MAX_LOG_SIZE) eventLog.shift();

  for (const res of connectedClients) {
    writeEvent(res, event);
  }

  console.log(
    `[SERVER] Evento "${event.type}" (id=${event.id}) enviado para ${connectedClients.length} cliente(s)`
  );
}

function generateEvent(): void {
  const event: ServerEvent = {
    id: nextEventId++,
    type: EVENT_TYPES[Math.floor(Math.random() * EVENT_TYPES.length)],
    payload: { value: Math.floor(Math.random() * 1000) },
    timestamp: new Date().toISOString(),
  };

  console.log(`\n[SERVER] Evento gerado: ${event.type}`);
  broadcast(event);

  setTimeout(generateEvent, 3_000 + Math.random() * 2_000);
}

function handleSSE(req: http.IncomingMessage, res: http.ServerResponse): void {
  res.writeHead(200, {
    "Content-Type": "text/event-stream",
    "Cache-Control": "no-cache",
    Connection: "keep-alive",
    "Access-Control-Allow-Origin": "*",
  });

  // diz ao EventSource (ou ao nosso cliente manual) quanto esperar antes de reconectar
  res.write(`retry: ${RETRY_MS}\n\n`);

  // Last-Event-ID: se o cliente está reconectando após uma queda, ele manda de
  // onde parou. Sem isso, todo drop de conexão significa perder eventos.
  const lastEventIdHeader = req.headers["last-event-id"];
  const lastEventId = Number(Array.isArray(lastEventIdHeader) ? lastEventIdHeader[0] : lastEventIdHeader) || 0;

  if (lastEventId > 0) {
    const missed = eventLog.filter((e) => e.id > lastEventId);
    console.log(
      `[SERVER] Cliente reconectou com Last-Event-ID=${lastEventId} → replaying ${missed.length} evento(s) perdido(s)`
    );
    for (const event of missed) writeEvent(res, event);
  }

  connectedClients.push(res);
  totalConnections++;
  console.log(
    `[SERVER] Cliente #${totalConnections} conectado | clientes ativos: ${connectedClients.length}`
  );

  // heartbeat: linha de comentário SSE (":") ignorada pelo parser, só existe
  // pra manter a conexão viva através de proxies/load balancers com timeout de idle
  const heartbeat = setInterval(() => {
    res.write(":\n\n");
  }, HEARTBEAT_MS);

  req.on("close", () => {
    clearInterval(heartbeat);
    const index = connectedClients.indexOf(res);
    if (index !== -1) connectedClients.splice(index, 1);
    console.log(`[SERVER] Cliente desconectou | clientes ativos: ${connectedClients.length}`);
  });
}

function handleReverseChannel(req: http.IncomingMessage, res: http.ServerResponse): void {
  let body = "";
  req.on("data", (chunk: Buffer) => (body += chunk.toString()));
  req.on("end", () => {
    try {
      const message = JSON.parse(body);
      console.log(`[SERVER] Mensagem recebida via canal reverso (POST /messages):`, message);
      res.writeHead(200, { "Content-Type": "application/json" });
      res.end(JSON.stringify({ ok: true }));
    } catch {
      res.writeHead(400);
      res.end();
    }
  });
}

const server = http.createServer((req, res) => {
  if (req.method === "GET" && (req.url === "/" || req.url === "/index.html")) {
    res.writeHead(200, { "Content-Type": "text/html; charset=utf-8" });
    res.end(indexHtml);
    return;
  }

  if (req.method === "GET" && req.url === "/events") {
    handleSSE(req, res);
    return;
  }

  if (req.method === "POST" && req.url === "/messages") {
    handleReverseChannel(req, res);
    return;
  }

  res.writeHead(404);
  res.end();
});

server.listen(3000, () => {
  console.log("[SERVER] Rodando na porta 3000");
  console.log(`[SERVER] retry=${RETRY_MS}ms | heartbeat a cada ${HEARTBEAT_MS / 1000}s`);
  console.log("[SERVER] Gerando eventos a cada 3-5 segundos...\n");
  setTimeout(generateEvent, 3_000);
});
