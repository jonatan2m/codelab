import http from "http";
import crypto from "crypto";

// "respostas do LLM" fake — mesmo texto do bonus-sse-chatbot, o que muda
// aqui é o padrão de conexão, não a geração em si
const CANNED_RESPONSES: string[] = [
  "Claro! SSE streama a resposta aos poucos, token a token, direto na mesma requisição que você fez — não precisa de uma conexão separada.",
  "Pensando em escala, o que importa não é o transporte em si, mas quantas inferências simultâneas seu backend consegue sustentar ao mesmo tempo.",
  "Cada resposta aqui é isolada por conversa. Isso significa que você não precisa de nenhum pub/sub entre instâncias — cada servidor resolve sozinho.",
  "Vou te explicar rapidinho: essa conversa inteira é só uma resposta HTTP que demora mais pra terminar, chegando em pedacinhos.",
];

type JobStatus = "running" | "done";

interface Job {
  id: string;
  message: string;
  tokens: string[]; // tudo que já foi gerado — é o que permite "pegar o bonde andando"
  status: JobStatus;
  subscribers: Set<http.ServerResponse>;
}

// TTL de um job concluído: depois disso, some da memória. Em produção seria
// Redis/DB, com TTL bem maior (ou permanente, se o histórico for um produto)
const JOB_TTL_MS = 60_000;

const jobs = new Map<string, Job>();

function broadcastToJob(job: Job, chunk: string): void {
  for (const res of job.subscribers) res.write(chunk);
}

function runGeneration(job: Job): void {
  const answer = CANNED_RESPONSES[Math.floor(Math.random() * CANNED_RESPONSES.length)];
  const words = answer.split(" ");
  let index = 0;

  function step(): void {
    if (index >= words.length) {
      job.status = "done";
      console.log(`[SERVER] Job ${job.id} concluído | assinantes no momento: ${job.subscribers.size}`);
      broadcastToJob(job, `event: done\ndata: {}\n\n`);
      for (const res of job.subscribers) res.end();
      job.subscribers.clear();
      setTimeout(() => jobs.delete(job.id), JOB_TTL_MS);
      return;
    }

    const token = words[index] + (index < words.length - 1 ? " " : "");
    job.tokens.push(token);
    broadcastToJob(job, `data: ${JSON.stringify({ token })}\n\n`);
    index++;

    setTimeout(step, 50 + Math.random() * 150);
  }

  // a geração roda independente de ter alguém ouvindo — é o ponto central
  // do padrão: o job não morre se o cliente desconectar no meio
  setTimeout(step, 50 + Math.random() * 150);
}

function handleCreateJob(req: http.IncomingMessage, res: http.ServerResponse): void {
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

    const job: Job = {
      id: crypto.randomUUID().slice(0, 8),
      message: parsed.message ?? "",
      tokens: [],
      status: "running",
      subscribers: new Set(),
    };
    jobs.set(job.id, job);
    console.log(`[SERVER] Job ${job.id} criado: "${job.message}"`);
    runGeneration(job);

    res.writeHead(200, { "Content-Type": "application/json" });
    res.end(JSON.stringify({ jobId: job.id }));
  });
}

function handleStreamJob(req: http.IncomingMessage, res: http.ServerResponse, jobId: string): void {
  const job = jobs.get(jobId);
  if (!job) {
    res.writeHead(404);
    res.end();
    return;
  }

  res.writeHead(200, {
    "Content-Type": "text/event-stream",
    "Cache-Control": "no-cache",
    Connection: "keep-alive",
    "Access-Control-Allow-Origin": "*",
  });

  // catch-up: manda tudo que já foi gerado antes desse cliente conectar (ou
  // reconectar) — é isso que evita perder o que já foi produzido no servidor
  if (job.tokens.length > 0) {
    console.log(
      `[SERVER] Cliente conectou no job ${jobId} → replaying ${job.tokens.length} token(s) já gerado(s)`
    );
    for (const token of job.tokens) {
      res.write(`data: ${JSON.stringify({ token })}\n\n`);
    }
  }

  if (job.status === "done") {
    res.write(`event: done\ndata: {}\n\n`);
    res.end();
    return;
  }

  job.subscribers.add(res);
  console.log(`[SERVER] Cliente assinou job ${jobId} | assinantes: ${job.subscribers.size}`);

  // mesmo gotcha do bonus-sse-chatbot: usar res.on("close") + writableEnded,
  // não req.on("close") (que dispara cedo demais assim que o body é lido)
  res.on("close", () => {
    if (!res.writableEnded) {
      job.subscribers.delete(res);
      console.log(
        `[SERVER] Cliente saiu do job ${jobId} (geração CONTINUA em segundo plano) | assinantes: ${job.subscribers.size}`
      );
    }
  });
}

const server = http.createServer((req, res) => {
  if (req.method === "POST" && req.url === "/jobs") {
    handleCreateJob(req, res);
    return;
  }

  const streamMatch = req.url?.match(/^\/jobs\/([a-zA-Z0-9-]+)\/stream$/);
  if (req.method === "GET" && streamMatch) {
    handleStreamJob(req, res, streamMatch[1]);
    return;
  }

  res.writeHead(404);
  res.end();
});

server.listen(3000, () => {
  console.log("[SERVER] Chatbot SSE (padrão job) rodando na porta 3000\n");
});
