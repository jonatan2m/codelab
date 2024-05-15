import streamlit as st
from langchain.vectorstores import FAISS
from langchain.embeddings import OpenAIEmbeddings
#from langchain.prompts imm

# Criação do formulário
form = st.form(key='my_form')

# Adição de campos ao formulário
name = form.text_input(label='Digite seu nome')
email = form.text_input(label='Digite seu email')
password = form.text_input(label='Digite sua senha', type='password')

# Adição de um botão de envio
submit_button = form.form_submit_button(label='Enviar')

# Lógica de processamento do formulário
if submit_button:
    st.write(f'Olá {name}! Seu formulário foi enviado com sucesso.')
