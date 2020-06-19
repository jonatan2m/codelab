using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Web3_1.Entities;

namespace Web3_1.Handlers
{
    public class NewMovieHandler : IRequestHandler<NewMovieCommand, Movie>
    {
        private readonly MovieRepository movieRepository;

        public NewMovieHandler(MovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<Movie> Handle(NewMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = new Movie
            {                
                Name = request.Name
            };

            movieRepository.Create(movie);

            return await Task.FromResult(movie);            
        }
    }
    

    public class NewMovieCommand : IRequest<Movie>
    {        
        public string Name { get; set; }
    }

    public class MovieRepository
    {
        public int Create(Movie movie)
        {
            movie.Id = new Random().Next();

            return movie.Id;
        }
    }
}
