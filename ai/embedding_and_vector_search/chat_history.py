import os
from dotenv import load_dotenv

from langchain_openai import ChatOpenAI, OpenAIEmbeddings
from langchain_community.vectorstores import Chroma
from langchain.memory import ConversationBufferMemory
from langchain.chains import ConversationalRetrievalChain
from PersistentConversationMemory import PersistentConversationMemory 

# Carregar variáveis de ambiente
load_dotenv()
openai_api_key = os.getenv("OPENAI_API_KEY")

# Inicializar embedding model
embedding_model = OpenAIEmbeddings(api_key=openai_api_key)

# Carregar base vetorial persistida
db = Chroma(persist_directory="chroma_db", embedding_function=embedding_model)

# Criar retriever a partir da base
retriever = db.as_retriever()

# Inicializar modelo de linguagem
llm = ChatOpenAI(
    api_key=openai_api_key,
    model="gpt-4-turbo",
    temperature=0  # ajusta criatividade
)

# Inicializar memória de conversa
# memory = ConversationBufferMemory(
#     memory_key="chat_history",
#     return_messages=True
# )

# Classe customizada para guardar o histórico num arquivo JSON
memory = PersistentConversationMemory(storage_path="./chat_history.json")

# Criar ConversationalRetrievalChain
chain = ConversationalRetrievalChain.from_llm(
    llm=llm,
    retriever=retriever,
    memory=memory
)

# Interações
while True:
    pergunta = input("\nSua pergunta (ou 'sair'): ")
    if pergunta.lower() == "sair":
        break

    resposta = chain.invoke({"question": pergunta})
    print(f"\n{resposta}")
