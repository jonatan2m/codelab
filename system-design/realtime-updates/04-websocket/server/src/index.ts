import http from "http";
import fs from "fs";
import path from "path";
import { WebSocketServer, WebSocket } from "ws";
import { createClient } from "redis";

const INSTANCE_ID = process.env.INSTANCE_ID ?? "?";
const REDIS_URL = process.env.REDIS_URL ?? "redis://redis:6379";
const CHANNEL = "chat-broadcast";

const indexHtmlTemplate = fs.readFileSync(path.join(__dirname, "../public/index.html"), "utf-8");
const indexHtml = indexHtmlTemplate.replace(/__INSTANCE_ID__/g, INSTANCE_ID);

const httpServer = http.createServer((req, res) => {
  if (req.method === "GET" && (req.url === "/" || req.url === "/index.html")) {
    res.writeHead(200, { "Content-Type": "text/html; charset=utf-8" });
    res.end(indexHtml);
    return;
  }
  res.writeHead(404);
  res.end();
});

// o mesmo servidor HTTP recebe o handshake de upgrade — não existe uma porta
// separada pro WebSocket, é a mesma conexão TCP mudando de protocolo
const wss = new WebSocketServer({ server: httpServer });

// clientes conectados NESTA instância — não há visibilidade das outras
const localClients = new Set<WebSocket>();

async function main(): Promise<void> {
  const publisher = createClient({ url: REDIS_URL });
  const subscriber = createClient({ url: REDIS_URL });

  publisher.on("error", (err) => console.error(`[SERVER ${INSTANCE_ID}] erro no publisher Redis:`, err.message));
  subscriber.on("error", (err) => console.error(`[SERVER ${INSTANCE_ID}] erro no subscriber Redis:`, err.message));

  await publisher.connect();
  await subscriber.connect();

  // é isso que resolve o problema de instâncias isoladas: quando QUALQUER
  // instância recebe uma mensagem de algum cliente, ela publica no Redis.
  // TODAS as instâncias (inclusive a que originou) estão inscritas nesse
  // canal e repassam pros próprios clientes locais.
  await subscriber.subscribe(CHANNEL, (message: string) => {
    const payload = JSON.parse(message);
    console.log(
      `[SERVER ${INSTANCE_ID}] recebido via Redis (originado na instância ${payload.instance}) → ` +
      `repassando pra ${localClients.size} cliente(s) local(is)`
    );
    for (const client of localClients) {
      if (client.readyState === WebSocket.OPEN) client.send(message);
    }
  });

  wss.on("connection", (ws) => {
    localClients.add(ws);
    console.log(`[SERVER ${INSTANCE_ID}] cliente conectado | clientes locais: ${localClients.size}`);

    ws.on("message", async (raw) => {
      let data: { from?: string; text?: string };
      try {
        data = JSON.parse(raw.toString());
      } catch {
        return;
      }

      console.log(`[SERVER ${INSTANCE_ID}] mensagem de "${data.from}": "${data.text}"`);

      const payload = JSON.stringify({
        from: data.from,
        text: data.text,
        instance: INSTANCE_ID,
        at: new Date().toISOString(),
      });

      // publica pro Redis em vez de mandar direto pros localClients — assim
      // TODAS as instâncias (via subscribe acima) tratam essa mensagem do
      // mesmo jeito, sem distinguir "minha" de "de outra instância"
      await publisher.publish(CHANNEL, payload);
    });

    ws.on("close", () => {
      localClients.delete(ws);
      console.log(`[SERVER ${INSTANCE_ID}] cliente desconectou | clientes locais: ${localClients.size}`);
    });
  });

  httpServer.listen(3000, () => {
    console.log(`[SERVER ${INSTANCE_ID}] rodando na porta 3000, conectado ao Redis em ${REDIS_URL}\n`);
  });
}

main().catch((err) => {
  console.error(`[SERVER ${INSTANCE_ID}] falha ao iniciar:`, err);
  process.exit(1);
});
