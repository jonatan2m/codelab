from langchain_community.document_loaders import PyPDFLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain_community.vectorstores import Chroma
from langchain_huggingface import HuggingFaceEmbeddings

# Caminho do PDF
pdf_path = "./arquivos/livro.pdf"

# Carregar PDF
loader = PyPDFLoader(pdf_path)
documentos = loader.load()

# Dividir o texto em pedaços menores
text_splitter = RecursiveCharacterTextSplitter(
    chunk_size=1000,
    chunk_overlap=200
)
docs_divididos = text_splitter.split_documents(documentos)

# Embedding local
embedding_model = HuggingFaceEmbeddings(model_name="sentence-transformers/all-MiniLM-L6-v2")

# Criar base vetorial
persist_directory = "./meu_livro_emb_minilm"
db = Chroma.from_documents(docs_divididos, embedding_model, persist_directory=persist_directory)

# Salvar base vetorial
db.persist()

print("Base vetorial criada e persistida com sucesso!")
