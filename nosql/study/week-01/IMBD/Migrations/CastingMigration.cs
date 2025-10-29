// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using IMBD.Database;
// using IMBD.Models;
// using MongoDB.Driver;

// namespace IMBD.Migrations
// {
//     public class CastingMigration
//     {
//         public static async Task MigrateMovieCastingsAsync(MongoDbContext db, CancellationToken cancellationToken = default)
//         {
//             // Abordagem mais simples - buscar todos os filmes e extrair os nomes únicos
//             var moviesWithCastings = await db.Movies
//                 .Find(m => m.Castings != null && m.Castings.Count > 0)
//                 .ToListAsync(cancellationToken);

//             // Extrair nomes únicos de atores
//             var uniqueActorNames = moviesWithCastings
//                 .SelectMany(m => m.Castings)
//                 .Distinct()
//                 .ToList();

//             var castingDictionary = new Dictionary<string, string>();

//             // Criar registros na collection Castings
//             foreach (var actorName in uniqueActorNames)
//             {
//                 var existing = await db.Castings
//                     .Find(c => c.Name == actorName)
//                     .FirstOrDefaultAsync(cancellationToken);

//                 if (existing == null)
//                 {
//                     var newCasting = new Casting { Name = actorName };
//                     await db.Castings.InsertOneAsync(newCasting, cancellationToken: cancellationToken);
//                     castingDictionary[actorName] = newCasting.Id;
//                 }
//                 else
//                 {
//                     castingDictionary[actorName] = existing.Id;
//                 }
//             }

//             // Atualizar Movies: converter Castings → Cast
//             foreach (var movie in moviesWithCastings.Where(m => m.Cast == null))
//             {
//                 var castReferences = movie.Castings
//                     .Where(name => castingDictionary.ContainsKey(name))
//                     .Select(name => new ActorReference 
//                     { 
//                         Id = castingDictionary[name], 
//                         Name = name 
//                     })
//                     .ToList();

//                 var update = Builders<Movie>.Update.Set(m => m.Cast, castReferences);
//                 await db.Movies.UpdateOneAsync(m => m.Id == movie.Id, update, cancellationToken: cancellationToken);
//             }
//         }
//     }
// }