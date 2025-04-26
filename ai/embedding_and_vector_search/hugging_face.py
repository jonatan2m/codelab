from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline

from langchain_chroma import Chroma
from langchain_huggingface import HuggingFaceEmbeddings, HuggingFacePipeline

# Carregar modelo e tokenizer GPT-Neo
model_name = "EleutherAI/gpt-neo-1.3B"
tokenizer = AutoTokenizer.from_pretrained(model_name)
model = AutoModelForCausalLM.from_pretrained(model_name, device_map="auto")

# Criar pipeline de geração de texto
text_gen_pipeline = pipeline(
    "text-generation",
    model=model,
    tokenizer=tokenizer,
    max_new_tokens=512,
    temperature=0.7,
    top_p=0.9,
    top_k=50
)

# Conectar pipeline ao LangChain
llm = HuggingFacePipeline(pipeline=text_gen_pipeline)

# Embeddings
embedding_model = HuggingFaceEmbeddings(model_name="sentence-transformers/all-MiniLM-L6-v2")

# Base vetorial
db = Chroma(
    persist_directory="./meu_livro_emb_minilm",
    embedding_function=embedding_model
    )

# Consulta na base + geração de resposta
def responder(pergunta):
    resultados = db.similarity_search(pergunta, k=5)
    contexto = "\n\n".join([doc.page_content for doc in resultados])

    prompt = f"""Baseando-se no texto abaixo, responda à pergunta de forma clara e completa.

Contexto:
{contexto}

Pergunta: {pergunta}
Resposta:"""

    resposta = llm.invoke(prompt)
    return resposta

# Exemplo de uso
print(responder("Quais são os níveis de memória que o GC tem?"))
