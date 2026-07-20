import http from "http";
import os from "os";

const SERVER_HOST = "server";
const SERVER_PORT = 3000;
// jitter opcional pra mitigar thundering herd: cada cliente espera um valor
// aleatório entre 0 e JITTER_MS antes de reconectar, em vez de todos ao mesmo tempo
const JITTER_MS = Number(process.env.JITTER_MS ?? 0);
const CLIENT_ID = os.hostname();

interface ServerEvent {
  id: number;
  type: string;
  payload: { value: number };
  timestamp: string;
}

interface PollResponse {
  event: ServerEvent;
}

let totalRequests = 0;
let totalEvents = 0;
let totalTimeouts = 0;
let requestStartTime = 0;

function poll(): void {
  totalRequests++;
  requestStartTime = Date.now();

  const options: http.RequestOptions = {
    hostname: SERVER_HOST,
    port: SERVER_PORT,
    path: "/poll",
    method: "GET",
    // aguarda até 35s (5s a mais que o timeout do servidor)
    timeout: 35_000,
  };

  console.log(`[CLIENT ${CLIENT_ID}] req #${totalRequests} → conectando e aguardando evento...`);

  const req = http.request(options, (res) => {
    let body = "";

    res.on("data", (chunk: Buffer) => (body += chunk.toString()));

    res.on("end", () => {
      const waitedMs = Date.now() - requestStartTime;

      if (res.statusCode === 200) {
        const data: PollResponse = JSON.parse(body);
        totalEvents++;
        console.log(
          `[CLIENT ${CLIENT_ID}] req #${totalRequests} → EVENTO recebido após ${waitedMs}ms: ${data.event.type}`
        );
        console.log(
          `[CLIENT ${CLIENT_ID}] Stats: ${totalRequests} reqs | ${totalEvents} eventos | ${totalTimeouts} timeouts`
        );
      } else if (res.statusCode === 204) {
        // timeout do servidor: nenhum evento em 30s, reconecta imediatamente
        totalTimeouts++;
        console.log(
          `[CLIENT ${CLIENT_ID}] req #${totalRequests} → timeout após ${waitedMs}ms (sem eventos) — reconectando`
        );
      }

      // reconecta após qualquer resposta — com jitter opcional pra não bater
      // todos os clientes no servidor no mesmo instante (thundering herd)
      // usando setTimeout e não setInterval: próxima req só sai após esta terminar
      const delay = JITTER_MS > 0 ? Math.random() * JITTER_MS : 0;
      setTimeout(poll, delay);
    });
  });

  req.on("timeout", () => {
    console.error(`[CLIENT ${CLIENT_ID}] Timeout de rede na req #${totalRequests} — reconectando`);
    req.destroy();
    setTimeout(poll, 1_000);
  });

  req.on("error", (err: Error) => {
    console.error(`[CLIENT ${CLIENT_ID}] Erro na req #${totalRequests}: ${err.message} — tentando em 2s`);
    setTimeout(poll, 2_000);
  });

  req.end();
}

// aguarda o servidor subir
setTimeout(() => {
  console.log(`[CLIENT ${CLIENT_ID}] Iniciando long polling | JITTER_MS=${JITTER_MS}\n`);
  poll();
}, 2_000);
