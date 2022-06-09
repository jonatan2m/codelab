using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using M220N.Models;
using M220N.Models.Projections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace M220N.Repositories
{
    public class MoviesRepository
    {
        private const int DefaultMoviesPerPage = 20;
        private const string DefaultSortKey = "tomatoes.viewer.numReviews";
        private const int DefaultSortOrder = -1;
        private readonly IMongoCollection<Comment> _commentsCollection;
        private readonly IMongoCollection<Movie> _moviesCollection;
        private readonly IMongoClient _mongoClient;

        private readonly BsonArray _ratingBoundaries = new BsonArray
        {
            0, 50, 70, 90, 100
        };

        private readonly BsonArray _runtimeBoundaries = new BsonArray
        {
            0, 60, 90, 120, 180
        };

        public MoviesRepository(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            var camelCaseConvention = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _moviesCollection = mongoClient.GetDatabase("sample_mflix").GetCollection<Movie>("movies");
            _commentsCollection = mongoClient.GetDatabase("sample_mflix").GetCollection<Comment>("comments");
        }

        /// <summary>
        ///     Get a <see cref="IReadOnlyList{T}" /> of <see cref="Movie" /> documents from the repository.
        /// </summary>
        /// <param name="moviesPerPage">The maximum number of movies to return.</param>
        /// <param name="page">
        ///     The page number, used in conjunction with <paramref name="moviesPerPage" /> to skip <see cref="Movie" />
        ///     documents for pagination.
        /// </param>
        /// <param name="sort">The field on which to sort the results.</param>
        /// <param name="sortDirection">
        ///     The direction to use when sorting the <see cref="Movie" />. 1 for ascending, -1 for
        ///     descending
        /// </param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns>A <see cref="IReadOnlyList{T}" /> of <see cref="Movie" /> objects.</returns>
        public async Task<IReadOnlyList<Movie>> GetMoviesAsync(int moviesPerPage = DefaultMoviesPerPage, int page = 0,
            string sort = DefaultSortKey, int sortDirection = DefaultSortOrder,
            CancellationToken cancellationToken = default)
        {
            var skip =  moviesPerPage * page;
            var limit = moviesPerPage;


            var sortFilter = new BsonDocument(sort, sortDirection);
            var movies = await _moviesCollection
                .Find(Builders<Movie>.Filter.Empty)
                .Limit(limit)
                .Skip(skip)
                .Sort(sortFilter)
                .ToListAsync(cancellationToken);

            return movies;
        }

        /// <summary>
        ///     Get a <see cref="Movie" />
        /// </summary>
        /// <param name="movieId">The Id of the <see cref="Movie" /></param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns>The <see cref="Movie" /></returns>
        public async Task<Movie> GetMovieAsync(string movieId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _moviesCollection.Aggregate()
                    .Match(Builders<Movie>.Filter.Eq(x => x.Id, movieId))
                    // Ticket: Get Comments
                    // Add a lookup stage that includes the
                    // comments associated with the retrieved movie
                    .Lookup(
                        _commentsCollection,
                        m => m.Id,
                        c => c.MovieId,
                        (Movie m) => m.Comments)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            catch (Exception ex)
            {
                // TODO Ticket: Error Handling
                // Catch the exception and check the exception type and message contents.
                // Return null if the exception is due to a bad/missing Id. Otherwise,
                // throw.

                throw;
            }
        }

        /// <summary>
        ///     For a given a country, return all the movies that match that country.
        /// </summary>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <param name="countries">An <see cref="Array" /> of countries to match.</param>
        /// <returns>A <see cref="IReadOnlyList{T}" /> of <see cref="MovieByCountryProjection" /> objects</returns>
        public async Task<IReadOnlyList<MovieByCountryProjection>> GetMoviesByCountryAsync(
            CancellationToken cancellationToken = default,
            params string[] countries
            )
        {
            // TODO Ticket: Projection - Search for movies by ``country`` and use projection to
            // return only the ``Id`` and ``Title`` fields
            //

            //return await _moviesCollection
            //   .Find(...)
            //   .Project(...)
            //   .ToListAsync(cancellationToken);

            //return null;

            var filter = Builders<Movie>.Filter.AnyIn(x => x.Countries, countries);
            var sort = Builders<Movie>.Sort.Descending(m => m.Title);
            var projection = Builders<Movie>.Projection.Include(x => x.Title);


            return await _moviesCollection
                .Find(filter)
                .Sort(sort)
                .Project<MovieByCountryProjection>(projection)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        ///     Finds all movies that contain the keyword(s).
        /// </summary>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <param name="limit">The maximum number of records to return</param>
        /// <param name="page">The page to return</param>
        /// <param name="keywords">The keywords on which to search movies</param>
        /// <returns>A List of <see cref="MovieByTextProjection" /> objects</returns>
        public async Task<IReadOnlyList<MovieByTextProjection>> GetMoviesByTextAsync(
            CancellationToken cancellationToken = default, int limit = DefaultMoviesPerPage,
            int page = 0, params string[] keywords)
        {
            var project = new BsonDocument("score", new BsonDocument("$meta", "textScore"));
            var sort = new BsonDocument("score", new BsonDocument("$meta", "textScore"));

            return await _moviesCollection
                .Find(Builders<Movie>.Filter.Text(string.Join(",", keywords)))
                .Project<MovieByTextProjection>(project)
                .Sort(sort)
                .Limit(limit)
                .Skip(page * limit)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        ///     Returns a list of Movies filtered by cast members.
        /// </summary>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <param name="sortKey">The field on which to sort the results</param>
        /// <param name="limit">The maximum number of results to return</param>
        /// <param name="page">The page to return</param>
        /// <param name="cast">one or more strings on which to search the "cast" field.</param>
        /// <returns>A List of <see cref="Movie" />s</returns>
        public async Task<IReadOnlyList<Movie>> GetMoviesByCastAsync(
            CancellationToken cancellationToken = default,
            string sortKey = DefaultSortKey,
            int limit = DefaultMoviesPerPage, int page = 0, params string[] cast)
        {
            var sort = new BsonDocument(sortKey, DefaultSortOrder);

            return await _moviesCollection
                .Find(Builders<Movie>.Filter.In("cast", cast))
                .Limit(limit)
                .Skip(page * limit)
                .Sort(sort)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        ///     Finds all movies that match the provide `genres`, sorted descending by the `sortKey` field.
        /// </summary>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <param name="sortKey">The field on which to sort the results</param>
        /// <param name="limit">The maximum number of results to return</param>
        /// <param name="page">The page to return</param>
        /// <param name="genres">The movie genres on which to filter the results</param>
        /// <returns>A List of Movies</returns>
        public async Task<IReadOnlyList<Movie>> GetMoviesByGenreAsync(
            CancellationToken cancellationToken = default,
            string sortKey = DefaultSortKey, int limit = DefaultMoviesPerPage,
            int page = 0, params string[] genres)
        {
            //var returnValue = new List<Movie>();

            var sort = new BsonDocument(sortKey, DefaultSortOrder);

            // TODO Ticket: Enable filtering of movies by genre.
            // If you get stuck see the ``GetMoviesByCastAsync`` method above.
            /*return await _moviesCollection
               .Find(...)
               .ToListAsync(cancellationToken);*/

            //DONE

            // // TODO Ticket: Paging
            // TODO Ticket: Paging
            // Modify the code you added in the Text and Subfield ticket to
            // include pagination. Refer to the other methods in this class
            // if you need a hint.

            var filter = Builders<Movie>.Filter.In("genres", genres);
            return await _moviesCollection
                .Find(filter)
                .Sort(sort)
                .Skip(page * DefaultMoviesPerPage)
                .Limit(DefaultMoviesPerPage)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        ///     Finds movies by cast members
        /// </summary>
        /// <param name="cast">The name of a cast member</param>
        /// <param name="page">The page to return</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns>A MoviesByCastProjection object</returns>
        public async Task<MoviesByCastProjection> GetMoviesCastFacetedAsync(string cast, int page = 0,
            CancellationToken cancellationToken = default)
        {
            /*
               TODO Ticket: Faceted Search

               We have already built the pipeline stages you need to perform a
               faceted search on the Movies collection. Your task is to append the
               facetStage, skipStage, and limitStage pipeline stages to the pipeline.
               Think carefully about the order that these stages should be executed!
           */

            // I match movies by cast members
            var matchStage = new BsonDocument("$match",
                new BsonDocument("cast",
                    new BsonDocument("$in",
                        new BsonArray {cast})));

            //I limit the number of results
            var limitStage = new BsonDocument("$limit", DefaultMoviesPerPage);

            //I sort the results by the number of reviewers, descending
            var sortStage = new BsonDocument("$sort",
                new BsonDocument("tomatoes.viewer.numReviews", -1));

            // In conjunction with limitStage, I enable pagination
            var skipStage = new BsonDocument("$skip", DefaultMoviesPerPage * page);

            // I build the facets
            var facetStage = BuildFacetStage();

            // I am the pipeline that runs all of the stages
            var pipeline = new[]
            {
                matchStage,
                sortStage,
                // add the remaining stages in the correct order
                skipStage,
                limitStage,
                facetStage

            };

            // I run the pipeline you built
            var result = await _moviesCollection
                .Aggregate(PipelineDefinition<Movie, MoviesByCastProjection>.Create(pipeline))
                .FirstOrDefaultAsync(cancellationToken);

            // We build another pipeline here to count the number of
            // movies that match _without_ the limit, skip, and facet stages
            var count = BuildAndRunCountPipeline(matchStage, sortStage);
            result.Count = (int) count.Values.First();

            return result;
        }

        /// <summary>
        ///     Helper method for building the Count pipeline
        /// </summary>
        /// <returns></returns>
        private BsonDocument BuildAndRunCountPipeline(BsonDocument matchStage, BsonDocument sortStage)
        {
            var countPipeline = new[]
            {
                matchStage,
                sortStage,
                new BsonDocument("$count", "count")
            };

            return _moviesCollection.Aggregate(PipelineDefinition<Movie, BsonDocument>.Create(countPipeline))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Helper method for building the Bucket stages
        /// </summary>
        /// <returns></returns>
        private BsonDocument BuildFacetStage()
        {            
            return new BsonDocument("$facet",
                new BsonDocument()
                {
                    BuildRuntimeBucketStage(),
                    BuildRatingBucketStage(),
                    BuildMoviesBucketStage()
                });
        }

        private BsonElement BuildRuntimeBucketStage()
        {
            return new BsonElement("runtime", new BsonArray
            {
                new BsonDocument("$bucket",
                    new BsonDocument
                    {
                        {"groupBy", "$runtime"},
                        {
                            "boundaries", _runtimeBoundaries
                        },
                        {"default", "other"},
                        {
                            "output",
                            new BsonDocument("count",
                                new BsonDocument("$sum", 1))
                        }
                    })
            });
        }

        private BsonElement BuildRatingBucketStage()
        {
            return new BsonElement("rating", new BsonArray
            {
                new BsonDocument("$bucket",
                    new BsonDocument
                    {
                        {"groupBy", "$metacritic"},
                        {
                            "boundaries", _ratingBoundaries
                        },
                        {"default", "other"},
                        {
                            "output",
                            new BsonDocument("count",
                                new BsonDocument("$sum", 1))
                        }
                    })
            });
        }

        private BsonElement BuildMoviesBucketStage()
        {
            return new BsonElement("movies",
                new BsonArray
                {
                    new BsonDocument("$addFields",
                        new BsonDocument("title", "$title"))
                });
        }

        /// <summary>
        ///     Gets the total number of movies
        /// </summary>
        /// <returns>A Long</returns>
        public async Task<long> GetMoviesCountAsync()
        {
            return await _moviesCollection.CountDocumentsAsync(Builders<Movie>.Filter.Empty);
        }

        private string GetMovieDocumentFieldType(string movieId, string fieldKey)
        {
            var fieldValue = GetMovieAsync(movieId).GetType().GetProperty(fieldKey);
            return fieldValue == null ? string.Empty : fieldValue.GetType().Name;
        }

        public ConfigInfo GetConfig()
        {
            var settings = _mongoClient.Settings;

            var command = new JsonCommand<ConfigInfo>("{ connectionStatus: 1, showPrivileges: true }");
            var authInfo = _commentsCollection.Database.RunCommand(command);

            authInfo.Settings = settings;
            return authInfo;
        }
    }
}
