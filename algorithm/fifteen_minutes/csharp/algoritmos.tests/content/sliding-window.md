# Sliding Window
Sliding Window é um algoritmo que funciona em cima de uma sequência que "anda" ao longo do tempo, para analise ou manter informações.

## Casos de uso na Industria

1. Monitoramento de desempenho e sistemas distribuídos.

    *Exemplo: Datadog, Prometheus, Elastic.*

    > Uso típico: calcular média móvel de CPU, memória, latência de requests em intervalos de 1m, 5m, 15m.

    O cálculo da média móvel ou percentis em tempo real geralmente usa Sliding Window para evitar recomputar todas as métricas históricas do zero.

2. Streaming de dados em tempo real

    *Exemplo: Apache Kafka + Flink / Spark Streaming.*

    Sliding Windows são nativas nesses frameworks (time-based windows).

    > Uso: detectar picos de acesso, calcular contagens por minuto, detectar fraudes (ex.: 10 transações em menos de 30s no mesmo cartão).

3. Networking (redes e protocolos)

    *Exemplo: TCP congestion control, protocolos de retransmissão.*

    > TCP usa um Sliding Window Protocol para controlar fluxo de pacotes, garantindo que o remetente não envie mais do que o receptor consegue processar.      
    Sem isso, redes congestionariam ou perderiam pacotes.

4. Processamento de sinais / multimídia

    *Exemplo: codecs de áudio/vídeo (MP3, H.264).*

    Operações como compressão e filtragem usam janelas deslizantes sobre frames ou amostras de áudio.

    > Uso prático: equalizadores de áudio, detecção de padrões em vídeo.

5. Segurança e detecção de anomalias

    *Exemplo: sistemas antifraude de bancos ou e-commerce.*

    Regra típica: "se houver mais de 5 tentativas de login em 1 minuto → bloquear".

    > A implementação interna faz um sliding window sobre os timestamps de login.

6. Busca e NLP (Processamento de Linguagem Natural)

    *Exemplo: motores de busca, análise de texto.*

    Sliding window é usada para gerar n-grams: ex., em “machine learning”, uma janela de 3 gera ["mac", "ach", "chi", …].

    > Aplicação: autocomplete, correção ortográfica, modelos de linguagem.

7. Analytics em apps e negócios

    *Exemplo: YouTube Analytics, Google Ads, e-commerce.*

    > Cálculo de KPIs em janelas móveis: média de watch time nos últimos 7 dias, taxa de cliques nas últimas 24h, etc.    
    Sem sliding window, cada cálculo teria de recomputar o histórico completo.