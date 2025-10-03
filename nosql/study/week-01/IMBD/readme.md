# Construir um IMDB fake.

Nesse projeto será criado um clone do imdb e os dados para utilização serão extraidos da própria api do imdb.

## Takeaways
Para coleções com relacionamento, como é o caso de Movie e Review, caso um filme possa ter muitos reviews, não é recomendado embutir.
Extrapola o limite de 16MB de um documento, não dá pra indexar um subdocumento dentro de arrays gigantes (fica muito lento).
Atualizar um array gigante gera Locks.
Melhor deixar coleções separadas e recorrer a paginação. 

    Endpoints distintos + paginação 
    GET /movies/{id} → retorna apenas os dados do filme.
    GET /movies/{id}/reviews?skip=0&limit=50 → pagina os reviews.
    Ou um endpoint combinado com includeReviews=true mas sempre paginado.
```bash
GET /api/movies/{id}/reviews?skip=0&limit=100
```
$lookup é para poucos registros, não serve pra esse cenário. Melhor fazer as queries separadas

### Performance
- Páginação + índices

    ```javascript
    db.reviews.createIndex({ movieId: 1 })
    db.reviews.createIndex({ movieId: 1, userId: 1 }) // se filtrar por usuário também
    ```
- Agregações resumidas:

    Calcule e guarde estatísticas do filme (countReviews, averageGrade, etc.) num documento separado ou no próprio movie para evitar full scan.
    
    Use $group e $avg offline para gerar dashboards.

- Shard / Particionamento:

    Se chegar a níveis absurdos, colocar a collection reviews shardeada por movieId (Cassandra style).

Para melhorar a performance e já trazer umas informações sobre os Reviews, é possível fazer uma colletion de suporte, a MovieStats, que vai guardar um compilado das informações dos reviews para exibição rápida em Dashboards

