FROM node as builder

RUN git clone https://github.com/phillies/vue_example.git /opt/vue_example
WORKDIR /opt/vue_example

RUN npm install && npm run build

FROM nginx

COPY --from=builder /opt/vue_example/dist/ /usr/share/nginx/html/

EXPOSE 80