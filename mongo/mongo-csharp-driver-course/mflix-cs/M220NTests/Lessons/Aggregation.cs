using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M220N.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NLessons
{
    /// <summary>
    /// This Test Class shows the code used in the Aggregation lesson.
    ///
    /// You learned about Querying in the previous
    /// lesson; in this lesson, we'll learn how to build an Aggregation pipeline
    /// with the C# driver. 
    /// 
    /// </summary>
    /// 
    public class Aggregation
    {

        private IMongoCollection<Movie> _moviesCollection;

        [SetUp]
        public void Setup()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            var client = new MongoClient(Constants.MongoDbConnectionUri());
            _moviesCollection = client.GetDatabase("sample_mflix").GetCollection<Movie>("movies");
        }

        /* Let's build an aggreation pipeline that:
         * 
         * 1. Finds all movies by a specific director (Rob Reiner),
         * 2. Sorts those movies by the number of reviews the movie has
         *    received on our site, and then
         * 3. Uses a projection to return only the movie title, the year the movie
         *    was released, and the average user rating.
         *
         *  Each of these steps will be built as its own stage, and then all
         *  three stages will be combined into a pipeline.
         */

        [Test]
        public void CountMovies()
        {
            // This stage finds all movies that have a specific director
            var matchStage = new BsonDocument("$match",
                new BsonDocument("directors", "Rob Reiner"));


            // This stage sorts the results by the number of reviews,
            // in descending order
            var sortStage = new BsonDocument("$sort",
                new BsonDocument("tomatoes.viewer.numReviews", -1));

            // This stage generates the projection we want
            var projectionStage = new BsonDocument("$project",
                new BsonDocument
                    {
                        { "_id", 0 },
                        { "Movie Title", "$title" },
                        { "Year", "$year" },
                        { "Average User Rating", "$tomatoes.viewer.rating" }
                    });

            /* We now put the stages together in a pipeline. Note that a
             * pipeline definition requires us to specify the input and output
             * types. In this case, the input is of type Movie, but because
             * we are using a Projection with custom fields, our output is
             * a generic BsonDocument object. To be really cool, we could
             * create a mapping class for the output type, which is what we've
             * done for you in the MFlix application.
             */
            
            var pipeline = PipelineDefinition<Movie, BsonDocument>
                .Create(new BsonDocument[] {
                    matchStage,
                    sortStage,
                    projectionStage
                });

            
            var result = _moviesCollection.Aggregate(pipeline).ToList();
            /* Note: we're making a synchronous Aggregate() call.
             * If you want a challenge, change the line above to make an
             * asynchronous call (hint: you'll need to make 2 changes),
             * and then confirm the unit test still passes.
             */

            Assert.AreEqual(14, result.Count);
            var firstMovie = result[0];
            Assert.AreEqual("The Princess Bride", firstMovie.GetValue("Movie Title").AsString);
            Assert.AreEqual(1987, firstMovie.GetValue("Year").AsInt32);
            Assert.AreEqual(4.0, firstMovie.GetValue("Average User Rating").AsDouble);

            /* We specifically excluded the "Id" field in the projection stage
             * that we built in the code above, so let's make sure that field
             * wasn't included in the resulting BsonDocument. We expect the call
             * to GetValue() to throw a KeyNotFoundException exception if the
             * field doesn't exist. 
             */

            Assert.Throws<KeyNotFoundException>(()=> firstMovie.GetValue("Id"));
        }
    }
}
