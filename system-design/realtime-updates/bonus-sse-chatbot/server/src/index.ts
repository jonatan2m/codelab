import http from "http";
import crypto from "crypto";

// "respostas do LLM" fake — o ponto do experimento não é gerar texto de
// verdade, é mostrar o streaming token a token isolado por conversa
const CANNED_RESPONSES: string[] = [
  "Claro! SSE streama a resposta aos poucos, token a token, direto na mesma requisição que você fez — não precisa de uma conexão separada.",
  "Pensando em escala, o que importa não é o transporte em si, mas quantas inferências simultâneas seu backend consegue sustentar ao mesmo tempo.",
  "Cada resposta aqui é isolada por conversa. Isso significa que você não precisa de nenhum pub/sub entre instâncias — cada servidor resolve sozinho.",
  "Vou te explicar rapidinho: essa conversa inteira é só uma resposta HTTP que demora mais pra terminar, chegando em pedacinhos.",
];

let activeStreams = 0;
let totalConversations = 0;

function handleChat(req: http.IncomingMessage, res: http.ServerResponse): void {
  let body = "";
  req.on("data", (chunk: Buffer) => (body += chunk.toString()));
  req.on("end", () => {
    let parsed: { message?: string };
    try {
      parsed = JSON.parse(body);
    } catch {
      res.writeHead(400);
      res.end();
      return;
    }

    const conversationId = crypto.randomUUID().slice(0, 8);
    totalConversations++;
    activeStreams++;
    console.log(
      `[SERVER] Conversa ${conversationId} iniciada: "${parsed.message}" | ` +
      `streams ativos: ${activeStreams} | total: ${totalConversations}`
    );

    res.writeHead(200, {
      "Content-Type": "text/event-stream",
      "Cache-Control": "no-cache",
      Connection: "keep-alive",
      "Access-Control-Allow-Origin": "*",
    });

    // "inferência": escolhe uma resposta e vai streamando token a token,
    // com delay aleatório simulando o ritmo de geração de um LLM real
    const answer = CANNED_RESPONSES[Math.floor(Math.random() * CANNED_RESPONSES.length)];
    const tokens = answer.split(" ");
    let index = 0;
    let closed = false;

    // gotcha do Node: "req" (a requisição) já teve seu body todo consumido
    // acima (data/end), e uma vez que um stream Readable é totalmente lido,
    // o Node emite "close" nele sozinho — mesmo com o socket TCP ainda vivo.
    // Se a gente escutasse req.on("close") aqui, ia disparar segundos depois
    // do "end", achando que o cliente caiu quando na real não caiu nada.
    // O sinal confiável de desconexão real é o "close" da RESPOSTA (res),
    // checando se ela terminou (writableEnded) antes desse evento disparar.
    res.on("close", () => {
      if (!closed && !res.writableEnded) {
        closed = true;
        activeStreams--;
        console.log(
          `[SERVER] Conversa ${conversationId} abortada (cliente desconectou) | streams ativos: ${activeStreams}`
        );
      }
    });

    function sendNextToken(): void {
      if (closed) return;

      if (index >= tokens.length) {
        res.write(`event: done\ndata: {}\n\n`);
        res.end();
        closed = true;
        activeStreams--;
        console.log(
          `[SERVER] Conversa ${conversationId} concluída | streams ativos: ${activeStreams}`
        );
        return;
      }

      const token = tokens[index] + (index < tokens.length - 1 ? " " : "");
      res.write(`data: ${JSON.stringify({ token })}\n\n`);
      index++;

      setTimeout(sendNextToken, 50 + Math.random() * 150);
    }

    sendNextToken();
  });
}

const server = http.createServer((req, res) => {
  if (req.method === "POST" && req.url === "/chat") {
    handleChat(req, res);
    return;
  }
  res.writeHead(404);
  res.end();
});

server.listen(3000, () => {
  console.log("[SERVER] Chatbot SSE rodando na porta 3000\n");
});
