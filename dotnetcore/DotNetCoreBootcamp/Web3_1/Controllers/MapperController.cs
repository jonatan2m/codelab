using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web3_1.FluentValidationExamples;
using Web3_1.MapperExamples;

namespace Web3_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapperController : ControllerBase
    {
        private readonly ILogger<MapperController> logger;
        private readonly IMapper _mapper;
        private readonly IValidator<EventViewModel> _validator;

        public MapperController(
            Microsoft.Extensions.Logging.ILogger<MapperController> logger,
            IMapper mapper,
            FluentValidation.IValidator<EventViewModel> validator)
        {
            this.logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Here is a summary example
        /// </summary>
        /// <returns>Item found</returns>        
        /// <response code="200">Find an item</response>
        /// <response code="500">An Error occurred</response>
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Index()
        {
            //agrupa todos os logs dentro do escopo com o mesmo TraceId
            using (logger.BeginScope("using a scope"))
            {
                logger.LogInformation("controller is configured. {Id}", Guid.NewGuid());
                

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


                logger.LogInformation("part 2. {Id}", Guid.NewGuid());
                

                return Ok(new { model, source });
            }
        }

        /// <summary>
        /// Process an event
        /// </summary>
        /// <returns>Entity created</returns>       
        /// <response code="200">Find an item</response>
        /// <response code="500">An Error occurred</response>
        [ProducesResponseType(typeof(Event), Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult Process(EventViewModel @event)
        {
            var result = _validator.Validate(@event);

            if (result.IsValid == false)
                return BadRequest(result.Errors);

            @event.Id = 100;
            var entity = _mapper.Map<EventViewModel, Event>(@event);

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created, entity);
        }
    }
}
