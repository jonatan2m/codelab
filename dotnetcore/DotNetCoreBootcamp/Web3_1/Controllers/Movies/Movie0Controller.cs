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
        private readonly IMediator _mediator;

        public Movie0Controller(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] NewMovieCommand command)
        {
            var result = _mediator.Send(command);
            return Ok(result);
        }
    }
}