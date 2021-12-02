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

        [HttpGet]
        public IActionResult Index()
        {
            var profile = new MapperProfile();
            IMapper map = new MapperConfiguration(c =>
            {
                c.AddProfile(profile);                
            }).CreateMapper();

            Event source = new Event
            {
                EventId = 1,
                CreatedAt = DateTime.Now,
                IsActive = true,
                Title = "Mapper Event Testing"
            };

            var model = map.Map<Event, EventViewModel>(source);

            return Ok(model);
        }
    }
}
