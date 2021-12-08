using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web3_1.MapperExamples;

namespace Web3_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapperController : ControllerBase
    {
        private readonly IMapper _mapper;

        public MapperController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Here is a summary example
        /// </summary>
        /// <returns>Item found</returns>
        /// <remarks>
        /// Sample result:
        ///     GET /
        ///     {
        ///         "eventId": 1,
        ///         "createdAt": DateTime.Now,
        ///         "isActive": true,
        ///         "Title": "Mapper Event Testing"
        ///     }
        /// </remarks>
        /// <response code="200">Find an item</response>
        /// <response code="500">An Error occurred</response>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Index()
        {
            //Injecting on DI, it isn't necessary
            //var profile = new MapperProfile();
            //IMapper map = new MapperConfiguration(c =>
            //{
            //    c.AddProfile(profile);                
            //}).CreateMapper();

            Event source = new Event
            {
                EventId = 1,
                CreatedAt = DateTime.Now,
                IsActive = true,
                Title = "Mapper Event Testing"
            };

            var model = _mapper.Map<Event, EventViewModel>(source);

            source = _mapper.Map<EventViewModel, Event>(model);

            return Ok(new { model, source });
        }
    }
}
