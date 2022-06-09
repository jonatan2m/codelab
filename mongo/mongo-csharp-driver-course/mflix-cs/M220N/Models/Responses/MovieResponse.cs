using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using M220N.Models.Projections;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace M220N.Models.Responses
{
    public class MovieResponse
    {
        private static readonly int MOVIES_PER_PAGE = 20;

        public MovieResponse(Movie movie)
        {
            if (movie != null)
            {
                Movie = movie;
                Api = "csharp";
                UpdatedType = (movie.LastUpdated is DateTime) ? "Date" : "Other";
            }
        }

        public MovieResponse(IReadOnlyList<Movie> movies, long totalMovieCount, int page,
            Dictionary<string, object> filters)
        {
            Movies = movies;
            MoviesCount = totalMovieCount;
            EntriesPerPage = MOVIES_PER_PAGE;
            Page = page;
            Filters = filters ?? new Dictionary<string, object>();
        }

        public MovieResponse(IReadOnlyList<MovieByCountryProjection> moviesByCountry, 
            long totalMovieCount, int page, Dictionary<string, object> filters)
        {
            Titles = moviesByCountry.Select(x => new KeyValuePair<ObjectId, string>(x.Id, x.Title)).ToList();
            EntriesPerPage = MOVIES_PER_PAGE;
            MoviesCount = totalMovieCount;
            Page = page;
            Filters = filters ?? new Dictionary<string, object>();
        }

        public MovieResponse(IReadOnlyList<MovieByTextProjection> movies, long totalMovieCount, int page,
            Dictionary<string, object> filters)
        {
            Movies = movies;
            MoviesCount = totalMovieCount;
            EntriesPerPage = MOVIES_PER_PAGE;
            Page = page;
            Filters = filters ?? new Dictionary<string, object>();
        }


        public MovieResponse(MoviesByCastProjection projection, int page, Dictionary<string, object> filters)
        {
            Movies = projection.Movies;
            Facets = new FacetClass(projection.Runtime, projection.Rating);
            EntriesPerPage = MOVIES_PER_PAGE;
            Page = page;
            MoviesCount = projection.Count;
            Filters = filters ?? new Dictionary<string, object>();
        }

        [JsonProperty("movie", NullValueHandling = NullValueHandling.Ignore)]
        public Movie Movie { get; set; }

        [JsonProperty("movies", NullValueHandling = NullValueHandling.Ignore)]
        public object Movies { get; set; }

        [JsonProperty("titles", NullValueHandling = NullValueHandling.Ignore)]
        public List<KeyValuePair<ObjectId, string>> Titles { get; set; }

        [JsonProperty("total_results", NullValueHandling = NullValueHandling.Ignore)]
        public long MoviesCount { get; set; }

        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int Page { get; set; }

        [JsonProperty("entriesPerPage", NullValueHandling = NullValueHandling.Ignore)]
        public int EntriesPerPage { get; set; }

        [JsonProperty("filters", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Filters { get; set; }

        [JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
        public string Api { get; set; }

        [JsonProperty("updated_type", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedType { get; set; }

        [JsonProperty("facets", NullValueHandling = NullValueHandling.Ignore)]
        public FacetClass Facets { get; set; }
    }

    public class FacetClass
    {
        public FacetClass(List<ExpandoObject> runtime, List<ExpandoObject> rating)
        {
            Runtime = runtime;
            Rating = rating;
        }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public List<ExpandoObject> Rating { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public List<ExpandoObject> Runtime { get; set; }
    }
}
