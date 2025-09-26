using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M220N.Controllers;
using M220N.Models;
using M220N.Models.Responses;
using M220N.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NTests
{
    internal class MovieControllerTests
    {
        private MovieController _movieController;

        [SetUp]
        public void Setup()
        {
            var client = new MongoClient(Constants.MongoDbConnectionUri());
            var movieRepository = new MoviesRepository(client);
            _movieController = new MovieController(movieRepository);
        }

        [Test]
        public async Task TestsGetMovieById()
        {
            var result = await _movieController.GetMovieAsync("573a1398f29313caabcea974");
            var okresult = (OkObjectResult) result;
            var movie = (MovieResponse)okresult.Value;
            Assert.AreEqual("The Princess Bride", movie.Movie.Title);

            //valid id, but no match
            result = await _movieController.GetMovieAsync("573a1398f29313caababc123");
            var badresult = (BadRequestObjectResult)result;
            var error = (ErrorResponse)badresult.Value;
            Assert.AreEqual("Not found", error.Error);

            //invalid id
            try
            {
                result = await _movieController.GetMovieAsync("9313caababc123");
            }
            catch (FormatException)
            {
                Assert.Pass("");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
           
        }

        [Test]
        public async Task TestFormattedOutputFromController()
        {
            var response = await _movieController.GetMoviesAsync();
            var jsonResult = response as OkObjectResult;
            Assert.IsNotNull(jsonResult);
            var responseObject = jsonResult.Value as MovieResponse;
            Assert.IsNotNull(responseObject);
            Assert.AreEqual(0, responseObject.Filters.Count);
            Assert.AreEqual(20, ((List<Movie>) responseObject.Movies).Count);
            Assert.AreEqual(23530, responseObject.MoviesCount);
            Assert.AreEqual(0, responseObject.Page);
        }

        [Test]
        public async Task TestFacetResults()
        {
            var response = await _movieController.GetMoviesCastFacetedAsync("Denzel Washington", 0);
            var jsonResult = response as OkObjectResult;
            Assert.IsNotNull(jsonResult);
            var responseObject = jsonResult.Value as MovieResponse;
            Assert.IsNotNull(responseObject);
            var facets = responseObject.Facets;
            Assert.AreEqual(20, ((List<Movie>) responseObject.Movies).Count);
            Assert.AreEqual(4, facets.Rating.Count);
            Assert.AreEqual(3, facets.Runtime.Count);

            response = await _movieController.GetMoviesCastFacetedAsync("Morgan Freeman", 2);
            jsonResult = response as OkObjectResult;
            Assert.IsNotNull(jsonResult);
            responseObject = jsonResult.Value as MovieResponse;
            Assert.IsNotNull(responseObject);
            facets = responseObject.Facets;
            Assert.AreEqual(8, ((List<Movie>) responseObject.Movies).Count);
            Assert.AreEqual(3, facets.Rating.Count);
            Assert.AreEqual(3, facets.Runtime.Count);
        }
    }
}
