using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web3_1.Controllers.Movies
{
    [Route("api/movies")]
    [ApiController]
    public class Movie1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMovies()
        {
            return Ok("Pega a visão");
        }
    }
}