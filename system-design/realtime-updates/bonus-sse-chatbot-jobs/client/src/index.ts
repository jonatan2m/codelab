import http from "http";
import os from "os";

const SERVER_HOST = "server";
const SERVER_PORT = 3000;
const USER_ID = os.hostname();

const QUESTIONS = [
  "Como funciona SSE?",
  "Isso escala pra milhares de usuários?",
  "Preciso de WebSocket pra isso?",
  "Qual a diferença pro long polling?",
];

function createJob(message: string): Promise<string> {
  return new Promise((resolve, reject) => {
    const payload = JSON.stringify({ message });
    const req = http.request(
      {
        hostname: SERVER_HOST,
        port: SERVER_PORT,
        path: "/jobs",
        method: "POST",
        headers: { "Content-Type": "application/json", "Content-Length": Buffer.byteLength(payload) },
      },
      (res) => {
        let body = "";
        res.on("data", (chunk: Buffer) => (body += chunk.toString()));
        res.on("end", () => {
          const parsed: { jobId: string } = JSON.parse(body);
          resolve(parsed.jobId);
        });
      }
    );
    req.on("error", reject);
    req.end(payload);
  });
}

// conecta no stream de um job. Se simulateDrop=true, o cliente aborta a
// conexão de propósito após alguns tokens — simulando um app que travou,
// aba que fechou ou rede que caiu no meio da resposta
function streamJob(jobId: string, simulateDrop: boolean): Promise<void> {
  return new Promise((resolve) => {
    let tokensThisConnection = 0;

    const req = http.request(
      { hostname: SERVER_HOST, port: SERVER_PORT, path: `/jobs/${jobId}/stream`, method: "GET" },
      (res) => {
        let buffer = "";
        res.setEncoding("utf8");

        res.on("data", (chunk: string) => {
          buffer += chunk;
          let separatorIndex: number;

          while ((separatorIndex = buffer.indexOf("\n\n")) !== -1) {
            const rawMessage = buffer.slice(0, separatorIndex);
            buffer = buffer.slice(separatorIndex + 2);

            let eventType = "message";
            let data = "";
            for (const line of rawMessage.split("\n")) {
              if (line.startsWith("event:")) eventType = line.slice("event:".length).trim();
              if (line.startsWith("data:")) data += line.slice("data:".length).trim();
            }

            if (eventType === "done") {
              console.log(`[USER ${USER_ID}] job ${jobId} concluído`);
              resolve();
              return;
            }
            if (!data) continue;

            const parsed: { token: string } = JSON.parse(data);
            tokensThisConnection++;
            process.stdout.write(`[USER ${USER_ID}] job ${jobId} token: "${parsed.token}"\n`);

            if (simulateDrop && tokensThisConnection === 3) {
              console.log(
                `[USER ${USER_ID}] simulando queda de conexão no job ${jobId} (após 3 tokens recebidos)`
              );
              req.destroy();
              resolve();
              return;
            }
          }
        });

        res.on("end", () => resolve());
      }
    );

    req.on("error", () => resolve());
    req.end();
  });
}

async function conversationTurn(): Promise<void> {
  const message = QUESTIONS[Math.floor(Math.random() * QUESTIONS.length)];
  console.log(`\n[USER ${USER_ID}] pergunta: "${message}"`);

  const jobId = await createJob(message);
  console.log(`[USER ${USER_ID}] job criado: ${jobId}`);

  // metade das conversas simula uma queda de conexão, pra provar que o job
  // continua gerando no servidor mesmo sem ninguém ouvindo
  const shouldSimulateDrop = Math.random() < 0.5;
  await streamJob(jobId, shouldSimulateDrop);

  if (shouldSimulateDrop) {
    const waitMs = 1_500 + Math.random() * 1_500;
    console.log(
      `[USER ${USER_ID}] "geração continua no servidor" — reconectando no job ${jobId} em ${Math.round(waitMs)}ms...`
    );
    await new Promise((resolve) => setTimeout(resolve, waitMs));
    await streamJob(jobId, false); // reconecta e recebe o catch-up + o resto ao vivo
  }

  scheduleNextTurn();
}

function scheduleNextTurn(): void {
  const thinkTimeMs = 2_000 + Math.random() * 5_000;
  setTimeout(() => {
    conversationTurn();
  }, thinkTimeMs);
}

setTimeout(() => {
  conversationTurn();
}, 2_000 + Math.random() * 2_000);
