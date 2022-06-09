using System.Linq;
using M220N.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NLessons
{
    public class Lookups
    {
        private IMongoCollection<Movie> _moviesCollection;
        private IMongoCollection<Comment> _commentsCollection;

        [SetUp]
        public void Setup()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            var client = new MongoClient(Constants.MongoDbConnectionUri());
            _moviesCollection = client.GetDatabase("sample_mflix").GetCollection<Movie>("movies");
            _commentsCollection = client.GetDatabase("sample_mflix").GetCollection<Comment>("comments");
        }

        /*
         *  Performing a Lookup with the Driver
         * 
         *  Now that we have seen how we can use Compass to build and export an aggregation pipeline, 
         *  let’s take a look at how we use that code with the C# driver. 
         *  Here’s the code that Compass generated for us, wrapped in an 
         *  array of BsonDocuments:
        */
        [Test]
        public void GetMovieWithCommentCount1()
        {
            var filter = new BsonDocument[]
            {
                new BsonDocument("$match",
                    new BsonDocument("year",
                        new BsonDocument
                        {
                            { "$gte", 1980 },
                            { "$lt", 1990 }
                        })),
                new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "comments" },
                        { "let", new BsonDocument("id", "$_id") },
                        { "pipeline",
                        new BsonArray
                            {
                                new BsonDocument("$match",
                                new BsonDocument("$expr",
                                new BsonDocument("$eq",
                                new BsonArray
                                {
                                    "$movie_id",
                                    "$$id"
                                }))),
                                new BsonDocument("$count", "count")
                            }},
                        { "as", "movie_comments" }
                    })
            };

            /* We can then define that as a PipelineDefinition object, 
             * which we pass to the Aggregate() function on the Movies 
             * collection:
             */

            var pipeline = PipelineDefinition<Movie, BsonDocument>
                .Create(filter);

            var movies = _moviesCollection.Aggregate(pipeline).ToList();

            var firstMovie = movies.First();

            Assert.AreEqual(2081, movies.Count);
            var comments = (BsonDocument)firstMovie.GetValue("movie_comments")[0];
            var count = (int)comments.GetValue("count");
            Assert.AreEqual(1, count);

            /* And we can see that it works. But this code isn't exactly 
             * easy to debug or change, should we need to. As 
             * we've already discussed in this course, there are multiple ways 
             * to accomplish tasks with the driver, so let's look at an 
             * approach that makes more sense and is easy to decipher:
             */
        }


        [Test]
        public void GetMovieWithCommentCount2()
        {
            /* Now here is an approach that uses several helpful methods in the 
             * driver: Aggregate(), Match(), and Lookup(), each of which is 
             * represented in the code above as MQL. The Lookup() method is a bit 
             * complex; here is an explanation of each of the parameters:
             * 
             *  - The collection from which we want to lookup the values 
             *      (in this case, the Comments collection)
             *  - The "key" in the Movies collection that will match a key in 
             *      the Comments collection.
             *  - The "key" in the Comments collection that matches the previous 
             *      parameter. In both cases, it's the _id of each Movie we match 
             *      in the Match state.
             *  - The property in which we want to put the lookup results. We 
             *      have already defined a Comments property on the Movie object 
             *      for just this purpose, so we specify it here.
             *  
             */
            var movies = _moviesCollection
                .Aggregate()
                .Match(m => (int)m.Year < 1990 && (int)m.Year >= 1980)
                .Lookup(
                    _commentsCollection,
                    m => m.Id,
                    c => c.MovieId,
                    (Movie m)=>m.Comments
                    )
                .ToList();

            var firstMovie = movies.First();

            Assert.AreEqual(2081, movies.Count);
            Assert.AreEqual(1, firstMovie.Comments.Count);

            /* As you can see, this code is much cleaner. If you are looking 
             * carefully, you may notice that we are casting the Year value 
             * to an integer. The reason for this will become clear near the 
             * end of the course when we do some bulk data changes.
             */
        }
    }
}
