using System.Linq;
using System.Threading.Tasks;
using M220N.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NLessons
{
    /// <summary>
    /// This Test Class shows the code used in the Advanced Reads lesson.
    ///
    /// In this lesson, we'll look at a few different ways we can read data from
    /// MongoDB by using the driver. We'll start with the most basic tools and
    /// then show some more advanced querying.
    /// 
    /// </summary>
    public class AdvancedReads
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
        public async Task GetMoviesAsync()
        {
            /*
             * In the last lesson, we saw how to use the driver to do basic reads
             * and projections from the datastore using the Find method. In this
             * lesson, we're going to expand on that: we'll look at other helper
             * methods available to you when using Find, and then we'll look at
             * how to implement an Aggregation Pipeline with the driver.
             */

            /* Let's start with a filter that will return many movies. We'll
             * find all movies that have Tom Hanks listed in the cast:
             */

            var filter = Builders<Movie>.Filter.In("cast", new string[] { "Tom Hanks" });

            /*
             * In the last lesson, we saw that we can call Find and return a
             * List<Movie> that contains all of the results:
             *  
             *  var movies = await _moviesCollection.Find<Movie>(filter).ToListAsync();
             *
             * But what if we want to limit the number of results? While we add
             * server-side or client-side code to limit the number of results
             * shown to the user, it makes more sense to ask MongoDB to only
             * return the data that we need. To do this, we simply chain the Limit
             * method to the Find call, like this:
             */

            var movies = await _moviesCollection.Find<Movie>(filter)
                .Limit(2)
                .ToListAsync();

            Assert.AreEqual(2, movies.Count);

        }

        [Test]
        public async Task GetMoviesAdvancedAsync()
        {
            /*
             * Now let's implement paging, so that our front end app
             * will only show one page at a time.
             *
             * How many movies are on a page?
             * That's up to you and your app requirements.
             *
             * Sorting also matters:
             * when you do pagination, you usually don't want a random order.
             *
             * So, in this example, we'll ask for all of the movies, sorted by
             * the year each came out, but we want to show only 10 movies per page.
             * The client code is responsible for telling us which page to show,
             * and typically a method like this would include an input parameter
             * of type int for that.
             */

            var pageNumber = 3;

            var moviesPerPage = 10;

            var movies = await _moviesCollection.Find<Movie>(Builders<Movie>.Filter.Empty)
                .Sort(new BsonDocument("year", 1))
                .Limit(moviesPerPage)
                .Skip(pageNumber * moviesPerPage)
                .ToListAsync();

            /*
             * As with Queries and Projections, we can build the Sort using a
             * Builder method, like this:
             */

            var sortByYearDescending = Builders<Movie>.Sort.Ascending(m => m.Year);

            movies = await _moviesCollection.Find<Movie>(Builders<Movie>.Filter.Empty)
                .Sort(sortByYearDescending)
                .Limit(moviesPerPage)
                .Skip(pageNumber * moviesPerPage)
                .ToListAsync();

            // We expect 10 movies on this page, and the first should be older
            // -- or the same year -- as the last.
            Assert.AreEqual(10, movies.Count);
            Assert.LessOrEqual((int)movies.First().Year, (int)movies.Last().Year);
        }
    }
}
