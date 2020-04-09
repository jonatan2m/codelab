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
        public Task<Movie> Handle(NewMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = new Movie
            {
                Id = request.Id,
                Name = request.Name
            };

            //Criar

            return Task.FromResult(movie);}
    }

    public class NewMovieCommand : IRequest<Movie>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
