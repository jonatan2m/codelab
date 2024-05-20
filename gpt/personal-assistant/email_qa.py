import streamlit as st
from langchain_community.vectorstores import FAISS
from langchain_community.embeddings import OpenAIEmbeddings
from langchain.prompts import PromptTemplate
from langchain_community.chat_models import ChatOpenAI
from langchain.chains import LLMChain
from dotenv import load_dotenv
from langchain_community.document_loaders import CSVLoader

load_dotenv()

loader = CSVLoader(file_path="dataset.csv")
documents = loader.load()

embeddings = OpenAIEmbeddings()
db = FAISS.from_documents(documents, embeddings)

def retrieve_info(query):
    similar_response = db.similarity_search(query, k=3)
    return [doc.page_content for doc in similar_response]

#print(retrieve_info("the follow phrase is a spam or a ham? adam l beberg wrote on tue 1635465 sep 1635465 kragen sitaker wrote entire post included yep he sure did but thanks for reminding us"))

#temperature 1 muito criativo, 0 respostas padrão
llm = ChatOpenAI(temperature=0, model="gpt-3.5-turbo-16k-0613")

template = """
Você é um assistente virtual que trabalha numa empresa que tem o foco em identificar se uma determinada mensagem pode ser spam ou ham.
Sua função é identificar se uma mensagem pode ser spam ou não. Nossos operadores irão te passar uma mensagem e você vai dizer se é um spam ou não.

Vou lhe passar algumas mensagens anteriores e a classificação delas, pra você conseguir saber a classificação exata delas.

Aqui está uma mensagem recebida e que precisamos saber se ela é spam ou não
{message}

Aqui está a lista de mensagens anteriores. Essas mensagens estarão em inglês mas talvez cheguem mensagens em português pra você avaliar. Esse histórico servirá de base para você qualificar a mensagem que acabamos de receber.
{best_practice}

Escreva a resposta do modo mais preciso que souber.

"""

prompt = PromptTemplate(
    input_variables=["message", "best_practice"],
    template=template
)

chain = LLMChain(llm=llm, prompt=prompt)

def generate_response(message):
    best_practice = retrieve_info(message)
    response = chain.run(message=message, best_practice=best_practice)
    return response

print(generate_response("smile in pleasure smile in pain smile when trouble pours like rain smile when sum1 hurts u smile becoz someone still loves to see u smiling"))

# ham: adam l beberg wrote forwarding me stuff from a list is hardly handing me a job i was talking about the open reqs at kana the company i work for oh but programming in java is beneath you joe',
# ham: adam l beberg wrote fair use needs to be clarified a bit that s an understatement how else do i ever have hope of finding a job working for someone that makes things people are supposed to 
    #drumroll pay for well you could damn well get a fucking better attitude i practically handed you a job the other week and you pissed all over me i m done helping you you have joined a very exclusive
    #club that up to now has only had my sister as a member joe url',
# ham: well beberg unless you re really into anime and actually hold true that dead people can send email i think geege s subject is just dandy especially since she removed herself from the hive that is aol
    #and placed herself unto another but hey geege i think its cute when he worries like that don t you ducks and runs bonus fork points if adam knows what anime i m refering to bb'


# Criação do formulário
#form = st.form(key='my_form')

# Adição de campos ao formulário
#name = form.text_input(label='Digite seu nome')
#email = form.text_input(label='Digite seu email')
#password = form.text_input(label='Digite sua senha', type='password')

# Adição de um botão de envio
#submit_button = form.form_submit_button(label='Enviar')

# Lógica de processamento do formulário
#if submit_button:
#    st.write(f'Olá {name}! Seu formulário foi enviado com sucesso.')
