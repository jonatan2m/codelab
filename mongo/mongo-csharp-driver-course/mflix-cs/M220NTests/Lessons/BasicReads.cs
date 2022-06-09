using System.Linq;
using System.Threading.Tasks;
using M220N.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NLessons
{
    /// <summary>
    /// This Test Class shows the code used in the Basic Reads lesson.
    ///
    /// In this lesson, we'll look at a few different ways we can read data from
    /// MongoDB by using the driver. We'll start with the most basic tools and
    /// then show some more advanced querying.
    /// 
    /// </summary>
    public class BasicReads
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

        [Test]
        public async Task GetOneMovieAsync()
        {
            /*
             * Let's begin with a simple query: we want to return a specific movie.
             * We know the title of the movie, so we should be able to query
             * the data store for all movies with a matching title. In theory,
             * there will only be one movie returned, but we'll check that assumption.
             *
             * First, we create the query filter. A BsonDocument takes in the field
             * name as the first paramter and the value as the second:
             */

            var filter = new BsonDocument("title", "The Princess Bride");

            /* Note that we are specifying the field name as a string, which
             * can be error-prone. We can make this a bit more type-safe
             * by using a Filter builder and LINQ, like this:
             */

            var betterFilter = Builders<Movie>.Filter.Eq(m => m.Title, "The Princess Bride");

            /*
             * Now we call Find, passing in the filter, and casting the results
             * as a List<Movie>. Note the *await* keyword!
             */
            var movies = await _moviesCollection.Find<Movie>(betterFilter).ToListAsync();

            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual("The Princess Bride", movies.First().Title);

            var movie = movies.First();
            /* Now, if we look at the information returned, the Movie document
             * is quite large. What if we want to only return a small subset of
             * the data in a Movie? We use MongoDB projection for that, and
             * a projection filter is created the same way as a query filter.
             * Let's ask the driver to return only the Title, Year, and Cast for
             * the movie. Note that the Id field is always included in a projection
             * unless we explicitly exclude it. We do not need to explicitly
             * exclude any other fields.
             */

            var projectionFilter = Builders<Movie>.Projection
                .Include(m => m.Title)
                .Include(m => m.Year)
                .Include(m => m.Cast)
                .Exclude(m => m.Id);

            /* This projection is the same as thi MQL expression:
             *
             * "{ title: 1, year: 1, cast: 1, _id: 0 }"
             */

            /* We'll use the same query filter, and since we already know that
             * the query filter returns a single document, we can call
             * FirstOrDefaultAsync() instead of casting to a List. So let's
             * add our Projection filter:
             */
            var movieProjected = await _moviesCollection
                .Find<Movie>(betterFilter)
                .Project<Movie>(projectionFilter)
                .FirstOrDefaultAsync();

            Assert.AreEqual("The Princess Bride", movieProjected.Title);
            Assert.AreEqual(1987, movieProjected.Year);
            Assert.AreEqual(4, movieProjected.Cast.Count);

            // Every other property should be null!
            Assert.IsNull(movieProjected.Id);
            Assert.IsNull(movieProjected.Awards);
        }
    }

}
