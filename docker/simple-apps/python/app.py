from flask import Flask, escape, request
import os

greeting = os.environ.get('GREETING', 'Hello')

app = Flask(__name__)

@app.route('/')
def hello():
    name = request.args.get('name', 'World')
    return f'{greeting}, {escape(name)}!'

# if __name__ == "__main__":
app.run(host="0.0.0.0", debug=True)