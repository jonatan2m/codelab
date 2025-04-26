import os
from dotenv import load_dotenv

from langchain_community.document_loaders import UnstructuredMarkdownLoader, PyPDFLoader, TextLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain_openai.embeddings import OpenAIEmbeddings
from langchain_community.vectorstores import Chroma

from langchain.chat_models import ChatOpenAI

# Carregar variáveis de ambiente
load_dotenv()
openai_api_key = os.getenv("OPENAI_API_KEY")

# Configurações do modelo de embedding e do GPT
embedding_model = OpenAIEmbeddings(api_key=openai_api_key)
chat_model = ChatOpenAI(openai_api_key=openai_api_key, model_name="gpt-4-turbo", temperature=0)

# Função para carregar texto de TXT
def carregar_txt(caminho):
    loader = TextLoader(caminho)
    return loader.load()

# Função para carregar texto de PDF
def carregar_pdf(caminho):
    loader = PyPDFLoader(caminho)
    return loader.load()

# Função para carregar texto de Markdown
def carregar_md(caminho):
    loader = UnstructuredMarkdownLoader(caminho)
    return loader.load()

# Função para criar base vetorial
def criar_base_vetorial(docs, nome_base):
    # Dividir documentos em pedaços menores    
    splitter = RecursiveCharacterTextSplitter(chunk_size=1000, chunk_overlap=100)
    documentos_divididos = splitter.split_documents(docs)

    db = Chroma.from_documents(documentos_divididos, embedding_model, persist_directory=nome_base)
    db.persist()
    return db

# Obter base criada
def obter_base_criada(nome_base):
    # Carregar base vetorial persistida
    db = Chroma(persist_directory=nome_base, embedding_function=embedding_model)
    return db

# Função para buscar conteúdo relacionado
def buscar_contexto(db, pergunta, k=3):
    resultados = db.similarity_search(pergunta, k=k)
    return "\n\n".join([r.page_content for r in resultados])

# Main
if __name__ == "__main__":
    print("Carregando arquivos...")

    #docs_txt = carregar_txt("arquivos/conteudo.txt")
    #docs_pdf = carregar_pdf("arquivos/livro.pdf")
    #docs_md  = carregar_md("arquivos/anotações.md")

    # documentos = docs_txt + docs_pdf + docs_md
    #documentos = docs_pdf

    #print("Criando base vetorial...")
    #db = criar_base_vetorial(documentos, "chroma_db")
    db = obter_base_criada("chroma_db")

    pergunta = "What is the Stack Roots."
    print(f"\nPergunta: {pergunta}")

    contexto = buscar_contexto(db, pergunta)
    print(f"\nContexto encontrado:\n{contexto}")

    resposta = chat_model.invoke(f"Baseado no conteúdo abaixo, responda:\n\n{contexto}\n\nPergunta: {pergunta}")
    print(f"\nResposta do GPT:\n{resposta}")
