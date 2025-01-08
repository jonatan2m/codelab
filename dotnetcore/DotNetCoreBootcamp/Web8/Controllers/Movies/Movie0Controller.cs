using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web3_1.Handlers;

namespace Web3_1.Controllers.Movies
{
    [Route("api/movies")]
    [ApiController]
    public class Movie0Controller : ControllerBase
    {
        private readonly EventDispacher _eventDispacher;
        private readonly IMediator _mediator;

        public Movie0Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Movie0Controller(EventDispacher eventDispacher)
        {
            _eventDispacher = eventDispacher;
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] NewMovieCommand command)
        {
            var result = _mediator.Send(command);

            _eventDispacher.Publish(new Ping() {Id = 2});
            return Ok("Nós que cria!");
        }
    }
}
