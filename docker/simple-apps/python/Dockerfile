FROM python:3.7-alpine
COPY requirements.txt requirements.txt

RUN pip install -r requirements.txt

ENV GREETING 'Hi'
ENV source_code /code

COPY . ${source_code}
WORKDIR ${source_code}

CMD python app.py
