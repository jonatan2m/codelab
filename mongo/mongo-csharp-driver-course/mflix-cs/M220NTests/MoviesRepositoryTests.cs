using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using M220N.Repositories;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NTests
{
    public class MoviesRepositoryTests
    {
        private MoviesRepository _movieRepository;
        private MongoClient _client;

        [SetUp]
        public void Setup()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _client = new MongoClient(Constants.MongoDbConnectionUri());
            _movieRepository = new MoviesRepository(_client);
        }

        [Test]
        public async Task TestReturnsOneMovie()
        {
            var movie = await _movieRepository.GetMovieAsync("573a1398f29313caabcea974");
            Assert.AreEqual("The Princess Bride", movie.Title);
        }

        [Test]
        public async Task TestReturnsNullForBadMovieId()
        {
            // not 24 characters
            var movie = await _movieRepository.GetMovieAsync("57caabcea974");
            Assert.IsNull(movie);
            // not valid characters
            movie = await _movieRepository.GetMovieAsync("im24characterslongbutbad");
            Assert.IsNull(movie);
            // null id
            movie = await _movieRepository.GetMovieAsync(null);
            Assert.IsNull(movie);
            // This is a valid id, but doesn't not exist in the collection. Should also return null.
            movie = await _movieRepository.GetMovieAsync("111a1398f29333caabcea999");
            Assert.IsNull(movie);
        }

        [Test]
        public async Task TestReturnsMoviesByDefaults()
        {
            //limit to 22 and skip to page 10 (skip == 220); use default sort
            var movies = await _movieRepository.GetMoviesAsync(22, 10);

            Assert.AreEqual(22, movies.Count);
            Assert.AreEqual("573a1399f29313caabced54b", movies.First().Id);
            Assert.AreEqual("Sister Act", movies.First().Title);
            Assert.AreEqual("The Uninvited", movies.Last().Title);

            // use all default settings of the GetMovies method
            movies = await _movieRepository.GetMoviesAsync();
            Assert.AreEqual(20, movies.Count);
            Assert.AreEqual("Titanic", movies.First().Title);
            Assert.AreEqual("School of Rock", movies.Last().Title);
        }

        [Test]
        public async Task TestReturnsMoviesSortedByCustom()
        {
            var movies = await _movieRepository.GetMoviesAsync(42, 10, "title");
            Assert.AreEqual(42, movies.Count);
            Assert.AreEqual("Wither", movies.First().Title);
            Assert.AreEqual(2012, movies.First().Year);
            Assert.AreEqual("Winter Sleepers", movies.Last().Title);
            Assert.AreEqual(1997, movies.Last().Year);

            movies = await _movieRepository.GetMoviesAsync(1, 0, "runtime");
            Assert.AreEqual("573a1397f29313caabce69db", movies.First().Id);
            Assert.AreEqual("Centennial", movies.First().Title);
            Assert.AreEqual(1256, movies.First().Runtime);
        }

        [Test]
        public async Task TestGetMoviesByKeywords()
        {
            var movies = await _movieRepository.GetMoviesByTextAsync(keywords: "caleb");
            Assert.AreEqual(12, movies.Count);
            Assert.AreEqual("573a13aef29313caabd2c3af", movies.First().Id);
            Assert.AreEqual("573a13c3f29313caabd6adf3", movies.Last().Id);

            movies = await _movieRepository.GetMoviesByTextAsync(keywords: "aliens, elvis, volcano");
            Assert.AreEqual(20, movies.Count);
            Assert.AreEqual("573a139af29313caabcf0729", movies.First().Id);
            Assert.AreEqual("573a13d0f29313caabd8b573", movies.Last().Id);
        }

        [Test]
        public async Task TestGetMoviesByGenre()
        {
            var movies = await _movieRepository.GetMoviesByGenreAsync(limit: 1000, genres: "Talk-Show");
            Assert.AreEqual(1, movies.Count);
        }

        [Test]
        public async Task TestGetMoviesByCast()
        {
            var movies = await _movieRepository.GetMoviesByCastAsync(limit: 40, cast: "George Clooney");
            Assert.AreEqual(29, movies.Count);

            movies = await _movieRepository.GetMoviesByCastAsync(limit: 40,
                cast: new[] {"Bud Cort", "Ruth Gordon"});
            Assert.AreEqual(10, movies.Count);
        }

        [Test]
        public async Task TestGetMoviesByCountry()
        {
            CancellationToken cancellationToken = default;
            var movies = await _movieRepository.GetMoviesByCountryAsync(cancellationToken, "Kosovo");
            Assert.AreEqual(2, movies.Count);
            Assert.AreEqual("Sworn Virgin", movies.First().Title);
            Assert.AreEqual("Babai", movies.Last().Title);

            movies = await _movieRepository.GetMoviesByCountryAsync(cancellationToken, "Russia", "Japan");
            Assert.AreEqual(1237, movies.Count);
            var firstMovie = movies.First();
            Assert.AreEqual("573a13f3f29313caabddf331", firstMovie.Id.ToString());
        }

        [Test]
        public async Task TestFacets()
        {
            var cast = "Salma Hayek";
            var moviesInfo = await _movieRepository.GetMoviesCastFacetedAsync(cast);
            Assert.AreEqual(20, moviesInfo.Movies.Count);
            Assert.AreEqual(3, moviesInfo.Runtime.Count);
            Assert.AreEqual(3, moviesInfo.Rating.Count);
            Assert.AreEqual(21, moviesInfo.Count);

            cast = "Ruth Gordon";
            moviesInfo = await _movieRepository.GetMoviesCastFacetedAsync(cast);
            Assert.AreEqual(4, moviesInfo.Movies.Count);
            Assert.AreEqual(3, moviesInfo.Runtime.Count);
            Assert.AreEqual(1, moviesInfo.Rating.Count);
            Assert.AreEqual(4, moviesInfo.Count);

            //test paging
            cast = "Charlize Theron";
            moviesInfo = await _movieRepository.GetMoviesCastFacetedAsync(cast, 1);
            Assert.AreEqual(3, moviesInfo.Movies.Count);
            Assert.AreEqual(2, moviesInfo.Runtime.Count);
            Assert.AreEqual(2, moviesInfo.Rating.Count);
            Assert.AreEqual(23, moviesInfo.Count);

            // A NullReferenceException should be thrown when cast field is empty
            try
            {
                await _movieRepository.GetMoviesCastFacetedAsync(string.Empty);
            }
            catch (NullReferenceException)
            {
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public async Task TestReturnsOneMovieWithComments()
        {
            var movie = await _movieRepository.GetMovieAsync("573a1391f29313caabcd6d40");
            Assert.AreEqual("High and Dizzy", movie.Title);
            Assert.AreEqual(1, movie.Comments.Count);
            Assert.AreEqual("Yolanda Owen", movie.Comments.First().Name);
        }

        [Test]
        public void TestVerifyConfigSettings()
        {
            _client = new MongoClient(Constants.MongoDbConnectionUriWithMaxPoolSize).WithWriteConcern(
                new WriteConcern(wTimeout: TimeSpan.FromMilliseconds(2500))) as MongoClient;

            _movieRepository = new MoviesRepository(_client);
            var config = _movieRepository.GetConfig();
            Assert.AreEqual(5, config.Settings.MaxConnectionPoolSize);
            Assert.AreEqual(2500, config.Settings.WriteConcern.WTimeout.Value.TotalMilliseconds);
        }

    }
}