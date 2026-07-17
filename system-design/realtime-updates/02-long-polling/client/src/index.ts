import http from "http";

const SERVER_HOST = "server";
const SERVER_PORT = 3000;

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

  console.log(`[CLIENT] req #${totalRequests} → conectando e aguardando evento...`);

  const req = http.request(options, (res) => {
    let body = "";

    res.on("data", (chunk: Buffer) => (body += chunk.toString()));

    res.on("end", () => {
      const waitedMs = Date.now() - requestStartTime;

      if (res.statusCode === 200) {
        const data: PollResponse = JSON.parse(body);
        totalEvents++;
        console.log(
          `[CLIENT] req #${totalRequests} → EVENTO recebido após ${waitedMs}ms: ${data.event.type}`
        );
        console.log(
          `[CLIENT] Stats: ${totalRequests} reqs | ${totalEvents} eventos | ${totalTimeouts} timeouts`
        );
      } else if (res.statusCode === 204) {
        // timeout do servidor: nenhum evento em 30s, reconecta imediatamente
        totalTimeouts++;
        console.log(
          `[CLIENT] req #${totalRequests} → timeout após ${waitedMs}ms (sem eventos) — reconectando`
        );
      }

      // reconecta imediatamente após qualquer resposta
      // usando setTimeout e não setInterval: próxima req só sai após esta terminar
      setTimeout(poll, 0);
    });
  });

  req.on("timeout", () => {
    console.error(`[CLIENT] Timeout de rede na req #${totalRequests} — reconectando`);
    req.destroy();
    setTimeout(poll, 1_000);
  });

  req.on("error", (err: Error) => {
    console.error(`[CLIENT] Erro na req #${totalRequests}: ${err.message} — tentando em 2s`);
    setTimeout(poll, 2_000);
  });

  req.end();
}

// aguarda o servidor subir
setTimeout(() => {
  console.log("[CLIENT] Iniciando long polling\n");
  poll();
}, 2_000);
