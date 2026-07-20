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

function askQuestion(): void {
  const message = QUESTIONS[Math.floor(Math.random() * QUESTIONS.length)];
  const payload = JSON.stringify({ message });
  const startedAt = Date.now();
  let firstTokenAt: number | null = null;
  let answer = "";

  console.log(`\n[USER ${USER_ID}] pergunta: "${message}"`);

  const req = http.request(
    {
      hostname: SERVER_HOST,
      port: SERVER_PORT,
      path: "/chat",
      method: "POST",
      headers: { "Content-Type": "application/json", "Content-Length": Buffer.byteLength(payload) },
    },
    (res) => {
      let buffer = "";
      res.setEncoding("utf8");

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
        const totalMs = Date.now() - startedAt;
        console.log(`\n[USER ${USER_ID}] resposta completa em ${totalMs}ms`);
        scheduleNextQuestion();
      });

      function handleMessage(rawMessage: string): void {
        let eventType = "message";
        let data = "";

        for (const line of rawMessage.split("\n")) {
          if (line.startsWith("event:")) eventType = line.slice("event:".length).trim();
          if (line.startsWith("data:")) data += line.slice("data:".length).trim();
        }

        if (eventType === "done" || !data) return;

        if (firstTokenAt === null) {
          firstTokenAt = Date.now();
          console.log(`[USER ${USER_ID}] primeiro token em ${firstTokenAt - startedAt}ms`);
        }

        const parsed: { token: string } = JSON.parse(data);
        answer += parsed.token;
        process.stdout.write(`[USER ${USER_ID}] token: "${parsed.token}"\n`);
      }
    }
  );

  req.on("error", (err: Error) => {
    console.error(`[USER ${USER_ID}] erro: ${err.message}`);
    scheduleNextQuestion();
  });

  req.end(payload);
}

function scheduleNextQuestion(): void {
  // o ritmo da conversa depende do usuário — pausa variável antes da próxima pergunta
  const thinkTimeMs = 2_000 + Math.random() * 5_000;
  setTimeout(askQuestion, thinkTimeMs);
}

// aguarda o servidor subir, com um delay inicial aleatório pra não sincronizar todo mundo
setTimeout(askQuestion, 2_000 + Math.random() * 2_000);
