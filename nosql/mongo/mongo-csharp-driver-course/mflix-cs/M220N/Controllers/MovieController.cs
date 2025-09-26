using System;
using System.Threading;
using System.Threading.Tasks;
using M220N.Models.Responses;
using M220N.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace M220N.Controllers
{
    public class MovieController : Controller
    {
        private readonly MoviesRepository _movieRepository;

        public MovieController(MoviesRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        /// <summary>
        ///     Returns the Movie with matching ID
        /// </summary>
        /// <param name="movieId">A string of the _id value of a movie</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns></returns>
        [HttpGet("api/v1/movies/getmovie/")]
        [HttpGet("api/v1/movies/id/{movieId}")]
        public async Task<ActionResult> GetMovieAsync(string movieId, CancellationToken cancellationToken = default)
        {
            var matchedMovie = await _movieRepository.GetMovieAsync(movieId, cancellationToken);
            if (matchedMovie == null) return BadRequest(new ErrorResponse("Not found"));
            return Ok(new MovieResponse(matchedMovie));
        }

        /// <summary>
        ///     Finds a specified number of movies documents in a given sort order.
        /// </summary>
        /// <param name="limit">The maximum number of documents to be returned. Defaults to 10.</param>
        /// <param name="page">The page number to retrieve. Defaults to 0.</param>
        /// <param name="sort">The sorting criteria. Defaults to "title".</param>
        /// <param name="sortDirection">The direction of the sort. Defaults to 1 (ascending).</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns></returns>
        [HttpGet("api/v1/movies/")]
        [HttpGet("api/v1/movies/search")]
        [HttpGet("api/v1/movies/getmovies")]
        public async Task<ActionResult> GetMoviesAsync(int limit = 20, [FromQuery(Name = "page")] int page = 0,
            string sort = "tomatoes.viewers.numReviews", int sortDirection = -1,
            CancellationToken cancellationToken = default)
        {
            var movies = await _movieRepository.GetMoviesAsync(limit, page, sort, sortDirection, cancellationToken);

            var movieCount = page == 0 ? await _movieRepository.GetMoviesCountAsync() : -1;
            return Ok(new MovieResponse(movies, movieCount, page, null));
        }

        /// <summary>
        ///     Return all movies that match one or more countries specified
        /// </summary>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <param name="countries">a comma-delimited list of country names</param>
        /// <returns>A BsonDocument that contains a list of matching movies</returns>
        [HttpGet("api/v1/movies/countries")]
        public async Task<ActionResult> GetMoviesByCountryAsync(
            CancellationToken cancellationToken = default,
            params string[] countries)
        {
            var movies = await _movieRepository.GetMoviesByCountryAsync(cancellationToken, countries);
            return Ok(new MovieResponse(movies, -1L, 0, null));
        }

        /// <summary>
        ///     Finds movies based on the provided search string
        /// </summary>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <param name="page">The page to return.</param>
        /// <param name="text">The text by which to search movies</param>
        /// <returns></returns>
        [HttpGet("api/v1/movies/search")]
        public async Task<ActionResult> GetMoviesByTextAsync(CancellationToken cancellationToken = default,
            int page = 0, [RequiredFromQuery] params string[] text)
        {
            var movies =
                await _movieRepository.GetMoviesByTextAsync(page: page, keywords: text,
                    cancellationToken: cancellationToken);
            return Ok(new MovieResponse(movies, await _movieRepository.GetMoviesCountAsync(), 0, null));
        }

        /// <summary>
        ///     Returns all movies that contain one or more of the specified cast members
        /// </summary>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <param name="page">The page to return</param>
        /// <param name="cast">The cast member(s) to search for</param>
        /// <returns></returns>
        [HttpGet("api/v1/movies/search")]
        public async Task<ActionResult> GetMoviesByCastAsync(CancellationToken cancellationToken = default,
            int page = 0, [RequiredFromQuery] params string[] cast)
        {
            //TODO Ticket: Subfield Text Search - implement the expected cast
            // filter and sort
            var movies =
                await _movieRepository.GetMoviesByCastAsync(page: page, cast: cast,
                    cancellationToken: cancellationToken);
            return Ok(new MovieResponse(movies, await _movieRepository.GetMoviesCountAsync(), 0, null));
        }

        /// <summary>
        ///     Finds movies by their genre
        /// </summary>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <param name="page">The page to return</param>
        /// <param name="genre">The genre(s) to filter by</param>
        /// <returns></returns>
        [HttpGet("api/v1/movies/search")]
        public async Task<ActionResult> GetMoviesByGenreAsync(CancellationToken cancellationToken = default,
            int page = 0, [RequiredFromQuery] params string[] genre)
        {
            var movies =
                await _movieRepository.GetMoviesByGenreAsync(page: page, genres: genre,
                    cancellationToken: cancellationToken);
            return Ok(new MovieResponse(movies, await _movieRepository.GetMoviesCountAsync(), 0, null));
        }

        /// <summary>
        ///     Finds movies by cast members.
        /// </summary>
        /// <param name="cast">The cast members to search for</param>
        /// <param name="page">The page to return</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns></returns>
        [HttpGet("api/v1/movies/facet-search")]
        public async Task<ActionResult> GetMoviesCastFacetedAsync(string cast, int page,
            CancellationToken cancellationToken = default)
        {
            var moviesInfo = await _movieRepository.GetMoviesCastFacetedAsync(cast, page, cancellationToken);
            return Ok(new MovieResponse(moviesInfo, page, null));
        }

        [HttpGet("api/v1/movies/config-options")]
        public ActionResult GetConfigOptions()
        {
            return Ok(new ConfigResponse(_movieRepository.GetConfig()));
        }
    }
}
