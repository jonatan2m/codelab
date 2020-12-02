from flask import Flask, escape, request
import requests
import os

port_app = os.environ.get('APP_PORT', '5005')

app = Flask(__name__)

@app.route('/')
def hello():    
    res = requests.get('http://api:5000?name=python-client')
    return f'Result, {res.text}!'
    

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=port_app)