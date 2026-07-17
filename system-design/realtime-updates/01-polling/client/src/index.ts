import http from "http";

const POLL_INTERVAL_MS = 1000; // tente mudar para 500 ou 2000 e observe o % de desperdício
const SERVER_HOST = "server";
const SERVER_PORT = 3000;

interface ServerEvent {
  id: number;
  type: string;
  payload: { value: number };
  timestamp: string;
}

interface PollResponse {
  events: ServerEvent[];
}

let totalRequests = 0;
let emptyResponses = 0;
let receivedEvents = 0;

function poll(): void {
  totalRequests++;

  const options: http.RequestOptions = {
    hostname: SERVER_HOST,
    port: SERVER_PORT,
    path: "/poll",
    method: "GET",
  };

  const req = http.request(options, (res) => {
    let body = "";

    res.on("data", (chunk: Buffer) => (body += chunk.toString()));

    res.on("end", () => {
      if (res.statusCode === 200) {
        const data: PollResponse = JSON.parse(body);
        receivedEvents += data.events.length;
        const wastePercent = Math.round((emptyResponses / totalRequests) * 100);

        console.log(
          `[CLIENT] req #${totalRequests} → DADOS: ${data.events.map((e) => e.type).join(", ")}`
        );
        console.log(
          `[CLIENT] Stats: ${totalRequests} reqs | ${emptyResponses} vazias (${wastePercent}% wasted) | ${receivedEvents} eventos`
        );
      } else {
        emptyResponses++;
        // Loga a cada 5 respostas vazias para não poluir o terminal
        if (emptyResponses % 5 === 0) {
          const wastePercent = Math.round((emptyResponses / totalRequests) * 100);
          console.log(
            `[CLIENT] req #${totalRequests} → vazia #${emptyResponses} — ${wastePercent}% das requisições são desperdício`
          );
        }
      }
    });
  });

  req.on("error", (err: Error) => {
    console.error(`[CLIENT] Erro na requisição: ${err.message}. Tentando novamente...`);
  });

  req.end();
}

// Aguarda o servidor subir antes de iniciar
setTimeout(() => {
  console.log(`[CLIENT] Iniciando polling a cada ${POLL_INTERVAL_MS}ms\n`);
  poll();
  setInterval(poll, POLL_INTERVAL_MS);
}, 2000);
