import http from "http";

type EventType = "new_order" | "payment_confirmed" | "user_signup";

interface ServerEvent {
  id: number;
  type: EventType;
  payload: { value: number };
  timestamp: string;
}

const eventQueue: ServerEvent[] = [];
let totalRequests = 0;
let emptyResponses = 0;
let dataResponses = 0;

const EVENT_TYPES: EventType[] = ["new_order", "payment_confirmed", "user_signup"];

function generateEvent(): void {
  const event: ServerEvent = {
    id: Date.now(),
    type: EVENT_TYPES[Math.floor(Math.random() * EVENT_TYPES.length)],
    payload: { value: Math.floor(Math.random() * 1000) },
    timestamp: new Date().toISOString(),
  };

  eventQueue.push(event);
  console.log(`[SERVER] Evento gerado: ${event.type} (fila: ${eventQueue.length})`);

  const nextMs = 3000 + Math.random() * 2000;
  setTimeout(generateEvent, nextMs);
}

const server = http.createServer((req, res) => {
  if (req.method !== "GET" || req.url !== "/poll") {
    res.writeHead(404);
    res.end();
    return;
  }

  totalRequests++;
  res.setHeader("Content-Type", "application/json");
  res.setHeader("Access-Control-Allow-Origin", "*");

  if (eventQueue.length > 0) {
    const batch = eventQueue.splice(0, eventQueue.length);
    dataResponses++;
    console.log(
      `[SERVER] req #${totalRequests} → 200 COM DADOS (${batch.length} evento(s)) | ` +
      `total: ${totalRequests} reqs, ${emptyResponses} vazias, ${dataResponses} com dados`
    );
    res.writeHead(200);
    res.end(JSON.stringify({ events: batch }));
  } else {
    emptyResponses++;
    console.log(
      `[SERVER] req #${totalRequests} → 204 VAZIA | ` +
      `total: ${totalRequests} reqs, ${emptyResponses} vazias (${Math.round((emptyResponses / totalRequests) * 100)}% wasted)`
    );
    res.writeHead(204);
    res.end();
  }
});

server.listen(3000, () => {
  console.log("[SERVER] Rodando na porta 3000");
  console.log("[SERVER] Gerando eventos a cada 3-5 segundos...\n");
  generateEvent();
});
