﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace consumer.Controllers
{
    [ApiController]
    [Route("/")]
    public class ConsumerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(ILogger<ConsumerController> logger)
        {
            _logger = logger;
        }     
    }
}
