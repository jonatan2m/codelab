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
    Quando temos um cluster shardeado (dados distribuídos entre servidores), o MongoDB divide os 
    dados em ranges baseados na Shard Key.
    Se a Shard Key for sequencial ou timestamp, um shard sempre vai ficar onerado, enquanto os outros ficarão ociosos. Fazer a distribuição uniforme pra resolve isso.

    Exemplo com IDs sequenciais:    
    ```bash
    // Shard Key: movieId (sequencial: 1, 2, 3, 4, 5...)
    // MongoDB divide em ranges:

    Shard A: movieId 1-1000
    Shard B: movieId 1001-2000  
    Shard C: movieId 2001-3000
    ```
    O que acontece com writes:
    ```bash
    // Novos filmes sempre têm IDs crescentes:
    movieId: 2001 → Shard C
    movieId: 2002 → Shard C  
    movieId: 2003 → Shard C
    movieId: 2004 → Shard C
    // Todos os writes vão para o Shard C!
    ```

    // 1. Definir shard key (obrigatório ter índice na shard key)
    sh.shardCollection("imdb.reviews", { movieId: 1 })

    // Isso AUTOMATICAMENTE cria o índice { movieId: 1 } se não existir
    // Mas é APENAS para permitir o sharding, não muda a distribuição

Para melhorar a performance e já trazer umas informações sobre os Reviews, é possível fazer uma colletion de suporte, a MovieStats, que vai guardar um compilado das informações dos reviews para exibição rápida em Dashboards

---
## tópicos especificos
### Indices

- Single-field (campo único) — índice simples ascendente/descendente.
    Ex.: db.collection.createIndex({ campo: 1 })

- Compound (composto) — índice em múltiplos campos (ordem importa).
    Ex.: db.collection.createIndex({ a: 1, b: -1 })

- Multikey — índice para campos que são arrays (indexa cada elemento).
    Ex.: db.collection.createIndex({ tags: 1 })

- Text — índice para busca textual (full‑text). Só é permitido um índice text por collection.
    Ex.: db.collection.createIndex({ descricao: "text" })

- Hashed — índice com hash do valor (útil para shard keys com distribuição uniforme).
    Ex.: db.collection.createIndex({ campo: "hashed" })

- Wildcard — índice que indexa campos dinâmicos/objetos aninhados (usar $).
    Ex.: db.collection.createIndex({ "$": 1 })

- Geospatial — índices espaciais: "2d" (planar) e "2dsphere" (geojson/superfície esférica).
    Ex.: db.collection.createIndex({ localizacao: "2dsphere" })

- TTL (Time To Live) — índice com expireAfterSeconds para expirar documentos automaticamente.
    Ex.: db.collection.createIndex({ createdAt: 1 }, { expireAfterSeconds: 3600 })

- Unique — índice que impõe unicidade de valores.
    Ex.: db.collection.createIndex({ email: 1 }, { unique: true })

- Partial — índice parcial que indexa apenas documentos que satisfaçam partialFilterExpression  (mais eficiente que sparse em muitos casos).
    Ex.: db.collection.createIndex({ ativo: 1 }, { partialFilterExpression: { ativo: true } })

- Sparse — índice que indexa somente documentos onde o campo existe (menos recomendado que partial em muitos cenários modernos).
    Ex.: db.collection.createIndex({ opcional: 1 }, { sparse: true })
---
## Desafios do projeto
Claro! Projetar algo como o IMDb — com milhões de usuários, filmes e reviews — é um excelente cenário para aprofundar seus estudos em banco de dados NoSQL.

A seguir está uma lista dos principais desafios reais que um projeto desse tipo enfrenta, organizados por categoria, com foco especial em NoSQL (como MongoDB, Cassandra, DynamoDB etc.):

🧱 1. Modelagem de dados flexível e escalável

Filmes com atributos diferentes:
Nem todo filme tem o mesmo conjunto de dados (ex: alguns têm premiações, outros não, etc.). Precisa de schema flexível.
    
✅ Como resolver:

- Documentos com campos opcionais	Casos simples, variação pequena
- Subdocumentos	Campos complexos e agrupados
- Campo "tipo"	Modelos muito diferentes entre si
Opção 1: Usar uma classe base + classes específicas + Factory

Você lê como BsonDocument, decide o tipo, e desserializa manualmente para o DTO certo.

```csharp
var raw = await _collection.Find<BsonDocument>(filtro).FirstOrDefaultAsync();
var tipo = raw["tipo"].AsString;

IMovieDto dto = tipo switch
{
    "documentario" => BsonSerializer.Deserialize<DocumentarioDto>(raw),
    "blockbuster" => BsonSerializer.Deserialize<BlockbusterDto>(raw),
    _ => BsonSerializer.Deserialize<MovieDto>(raw)
};
```

Opção 2: Ler como Movie, mas usar Tipo para projetar DTOs

Você mantém Movie como base de dados, mas mapeia para o DTO certo na camada de apresentação:
```csharp
var movie = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

IMovieDto dto = movie.Tipo switch
{
    "documentario" => new DocumentarioDto { Titulo = movie.Titulo, Tematica = movie.Tematica },
    "blockbuster" => new BlockbusterDto { Titulo = movie.Titulo, Bilheteria = movie.Bilheteria },
    _ => new MovieDto { Titulo = movie.Titulo }
};
```

- Dictionary<string, object> ou JSON dinâmico	Quando você não quer modelar tudo no código

Exemplo com ExpandoObject:
```csharp
dynamic dto = new ExpandoObject();
dto.Titulo = movie["titulo"];
dto.Diretor = movie["diretor"];

if (movie.Contains("premiacoes"))
    dto.Premiacoes = movie["premiacoes"].AsBsonDocument;

if (movie.Contains("bilheteria"))
    dto.Bilheteria = movie["bilheteria"].AsDouble;
```

ExpandoObject te permite adicionar ou omitir propriedades dinamicamente, e ainda assim retornar um JSON limpo na API.

Exemplo com Dictionary<string, object>:
```csharp
var dto = new Dictionary<string, object>();
dto["titulo"] = movie["titulo"];

if (movie.Contains("premiacoes"))
    dto["premiacoes"] = movie["premiacoes"];
```

Ideal se você quer expor um JSON dinâmico e tipado apenas como objeto para o frontend.


- DTOs separados por tipo de filme	Em APIs bem estruturadas com versionamento
declare um DTO com propriedades como:
```csharp
public class MovieDto
{
    public string Titulo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PremiacoesDto? Premiacoes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Bilheteria { get; set; }
}
```


Casting com múltiplos papéis:
Um ator pode ser diretor em um filme, protagonista em outro. Precisa modelar relacionamentos múltiplos.

✅ Como resolver:

Reviews, avaliações e comentários
    Enorme volume com estrutura simples, mas precisa de agregações e filtros rápidos.

✅ Como resolver:

Localização (i18n)
    Filmes e sinopses em múltiplos idiomas — estrutura de documentos por idioma ou collections separadas?

✅ Como resolver:

Dados duplicados (ou quase duplicados)
    Como lidar com mesmo filme listado em idiomas diferentes, ou atores com nomes semelhantes? Deduplicação é desafiadora.

🔄 2. Relacionamentos (sem joins nativos)

- Usuário → Reviews → Filme
    Mongo não tem joins nativos, apenas $lookup, então relacionamentos precisam ser planejados (embed? referência?).

- Filme → Castings
    Precisa decidir se vai embutir os dados ou referenciar — balancear performance com consistência.

- Ratings globais (nota média)
	Como calcular rapidamente sem trazer milhões de reviews? Use agregações e persistência de estatísticas.


📈 3. Escalabilidade
Desafio	Detalhes
Alta leitura de filmes populares	Filmes famosos (ex: "Vingadores") recebem milhares de acessos por minuto — leitura precisa ser rápida.
Escrita constante de reviews e votos	Precisa garantir throughput de escrita alto (Cassandra, MongoDB sharded, DynamoDB).
Paginação eficiente em grandes coleções	Exibir os primeiros 50 reviews entre milhões sem queda de performance.
📊 4. Consultas complexas e filtragens
Desafio	Detalhes
Filtrar por gênero, nota, ator, período	Consultas combinadas podem exigir índices compostos ou pipelines bem planejados.
Buscar por nome aproximado (fuzzy)	Ex: “Johnny Deep” → “Johnny Depp”. Solução: usar MongoDB Atlas Search, ElasticSearch ou Algolia.
Recomendações	Relacionamento implícito (usuários que gostaram deste também gostaram de...). Exige pré-processamento ou motor externo.
📦 5. Armazenamento de grandes volumes
Desafio	Detalhes
Limite de 16MB por documento (Mongo)	Precisa quebrar em subcoleções (reviews, comments, awards, etc.) para não ultrapassar.
Armazenar arquivos e imagens	Fotos de atores, pôsteres de filmes: salvar no banco (GridFS) ou usar S3-like?
Caches derivados	Ex: campos notaMedia, qtdeReviews, melhoresAtores guardados fora dos reviews para leitura rápida.
🔒 6. Consistência vs Performance
Desafio	Detalhes
Reviews escritos em paralelo	Como manter consistência eventual aceitável sem travar performance?
Nota média pode ficar desatualizada por minutos	Trade-off válido, mas precisa saber onde tolerar atraso.
Referência quebrada	Um review pode apontar para um filme ou usuário que foi deletado — precisa validar ou limpar.
🛠️ 7. Migração, versionamento e compatibilidade
Desafio	Detalhes
Schema versionado	Alterações em documentos exigem migração parcial ou versionamento (_v: 1).
Documentos legados com estrutura antiga	Precisa garantir compatibilidade em leitura e escrita.
Migração de dados entre clusters	Ex: mudar do Mongo local para Atlas ou Dynamo exige dump/index/config/restore.
🧠 8. Infraestrutura e otimização

- Sharding e balanceamento:	Para escalar horizontalmente.
    Ex: shard reviews por movieId.
- TTL para dados temporários:
	Ex: sessões, tokens, logs podem ser auto-expirados.
- Indexação pesada:
    Índices grandes demais degradam escrita — precisa balancear custo de escrita vs velocidade de leitura.

📌 Extras específicos de estudo em NoSQL
Tópico	Por que é relevante?
$lookup e seus limites	Join em Mongo, só funciona entre collections do mesmo banco e não sharded.
Agregações com $group, $sort, $limit, $facet	Para estatísticas e dashboards.
Estratégias de denormalização	Embutir ou referenciar? Quando e por quê.
Particionamento e estratégia de shard key	Fundamental em bancos distribuídos como Cassandra e MongoDB Sharded.
Indexação composta vs parcial vs TTL	Ajuda a reduzir carga e acelerar queries.
✅ Próximos passos sugeridos para seu estudo

Estudar modelagem de agregados e relacionamento entre collections com $lookup, referência e embed.

Simular carga real de dados (milhões de reviews) e aplicar agregações.

Usar pipelines para gerar estatísticas periódicas (movie_stats)

Praticar sharding e índices compostos, simulando filmes populares vs filmes obscuros.

Estudar Atlas Search ou ElasticSearch para busca fuzzy.

Explorar denormalização controlada para listas pequenas (topReviews, topCast).