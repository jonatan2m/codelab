# Instructions
pip install -U langchain langchain-openai langchain-community chromadb openai pypdf markdown python-dotenv



## Modelo rodando local pelo Hugging Face
- pip install transformers torch langchain
- pip install transformers accelerate sentence-transformers
- pip install langchain-huggingface 
- pip install langchain-chroma 


## Instalar o Docling para converter PDF em Markdown
- pip install docling

# Como utilizar
O que cada script faz:

- app.py: carrega um arquivo pdf, gera os embeddings na OpenAI, persiste o resultado numa base vetorial local (Chroma DB) e utiliza essa base como contexto para responder as perguntas, através da API da OpenAI. Dá suporte a texto e a markdown também

- chat_history.py: utiliza a base vetorial já criada e através da classe PersistentConversationMemory, consegue manter o histórico do chat. Também utilizando a OpenAI API

- gera_base_vetorial.py: serve para gerar a base localmente, só utilizando os recursos da HuggingFace

- hugging_face.py: Utiliza modelos disponveis na HuggingFace para dar as respostas. Atualmente minha máquina não consegue rodar qualquer modelo (inclusive o modelo que está aí não funciona muito bem).

- docling_test.py: Testando o docling pra converter o livro de PDF para markdown
