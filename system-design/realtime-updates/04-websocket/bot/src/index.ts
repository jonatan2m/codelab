import WebSocket from "ws";
import os from "os";

const TARGET_HOST = process.env.TARGET_HOST ?? "server-a";
const BOT_ID = `bot-${os.hostname()}`;

const MESSAGES = [
  "Alguém aí?",
  "Testando WebSocket bidirecional",
  "Essa mensagem deveria aparecer nas duas instâncias",
  "Redis Pub/Sub propagando certinho?",
];

function connect(): void {
  const ws = new WebSocket(`ws://${TARGET_HOST}:3000`);

  ws.on("open", () => {
    console.log(`[${BOT_ID}] conectado em ${TARGET_HOST}`);
    scheduleNextMessage(ws);
  });

  ws.on("message", (raw: Buffer) => {
    const data = JSON.parse(raw.toString());
    console.log(`[${BOT_ID}] recebeu de "${data.from}" (via instância ${data.instance}): "${data.text}"`);
  });

  ws.on("close", () => {
    console.log(`[${BOT_ID}] desconectado — reconectando em 2s`);
    setTimeout(connect, 2_000);
  });

  ws.on("error", (err: Error) => {
    console.error(`[${BOT_ID}] erro: ${err.message}`);
  });
}

function scheduleNextMessage(ws: WebSocket): void {
  const delay = 3_000 + Math.random() * 4_000;
  setTimeout(() => {
    if (ws.readyState === WebSocket.OPEN) {
      const text = MESSAGES[Math.floor(Math.random() * MESSAGES.length)];
      ws.send(JSON.stringify({ from: BOT_ID, text }));
      scheduleNextMessage(ws);
    }
  }, delay);
}

setTimeout(connect, 2_000 + Math.random() * 2_000);
