import http from "http";

type EventType = "new_order" | "payment_confirmed" | "user_signup";

interface ServerEvent {
  id: number;
  type: EventType;
  payload: { value: number };
  timestamp: string;
}

interface PendingClient {
  res: http.ServerResponse;
  connectedAt: number;
  timeoutHandle: NodeJS.Timeout;
}

const EVENT_TYPES: EventType[] = ["new_order", "payment_confirmed", "user_signup"];
const LONG_POLL_TIMEOUT_MS = 30_000;

const pendingClients: PendingClient[] = [];

let totalConnections = 0;
let totalEvents = 0;
let totalTimeouts = 0;
let totalDisconnects = 0;

function removePendingClient(client: PendingClient): void {
  const index = pendingClients.indexOf(client);
  if (index !== -1) {
    pendingClients.splice(index, 1);
    clearTimeout(client.timeoutHandle);
  }
}

function onEventArrived(event: ServerEvent): void {
  const snapshot = [...pendingClients];

  // responde todos os clientes que estão esperando
  for (const client of snapshot) {
    removePendingClient(client);
    const waitedMs = Date.now() - client.connectedAt;
    client.res.writeHead(200, { "Content-Type": "application/json" });
    client.res.end(JSON.stringify({ event }));
    console.log(`[SERVER] Cliente respondido após ${waitedMs}ms de espera`);
  }

  console.log(
    `[SERVER] Evento "${event.type}" entregue para ${snapshot.length} cliente(s) | ` +
    `fila atual: ${pendingClients.length}`
  );
}

function generateEvent(): void {
  const event: ServerEvent = {
    id: Date.now(),
    type: EVENT_TYPES[Math.floor(Math.random() * EVENT_TYPES.length)],
    payload: { value: Math.floor(Math.random() * 1000) },
    timestamp: new Date().toISOString(),
  };

  totalEvents++;
  console.log(`\n[SERVER] Evento gerado: ${event.type}`);
  onEventArrived(event);

  const nextMs = 3_000 + Math.random() * 2_000;
  setTimeout(generateEvent, nextMs);
}

const server = http.createServer((req, res) => {
  if (req.method !== "GET" || req.url !== "/poll") {
    res.writeHead(404);
    res.end();
    return;
  }

  totalConnections++;
  res.setHeader("Access-Control-Allow-Origin", "*");

  // timeout: nenhum evento em 30s → 204, cliente reconecta
  const timeoutHandle = setTimeout(() => {
    removePendingClient(client);
    totalTimeouts++;
    console.log(
      `[SERVER] Timeout para cliente #${totalConnections} após ${LONG_POLL_TIMEOUT_MS / 1000}s | ` +
      `timeouts: ${totalTimeouts}, fila: ${pendingClients.length}`
    );
    res.writeHead(204);
    res.end();
  }, LONG_POLL_TIMEOUT_MS);

  const client: PendingClient = {
    res,
    connectedAt: Date.now(),
    timeoutHandle,
  };

  pendingClients.push(client);

  // equivalente ao CancellationToken do C#: detecta cliente desconectado
  req.on("close", () => {
    if (pendingClients.includes(client)) {
      removePendingClient(client);
      totalDisconnects++;
      console.log(
        `[SERVER] Cliente desconectou antes de receber resposta | ` +
        `disconnects: ${totalDisconnects}, fila: ${pendingClients.length}`
      );
    }
  });

  console.log(
    `[SERVER] Cliente #${totalConnections} conectado e aguardando | fila: ${pendingClients.length}`
  );
});

server.listen(3000, () => {
  console.log("[SERVER] Rodando na porta 3000");
  console.log(`[SERVER] Timeout por cliente: ${LONG_POLL_TIMEOUT_MS / 1000}s`);
  console.log("[SERVER] Gerando eventos a cada 3-5 segundos...\n");
  setTimeout(generateEvent, 3_000);
});
