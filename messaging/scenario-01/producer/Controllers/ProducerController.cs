using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace producer.Controllers
{
    public class Operation
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public string OperationType { get; set; }
        public string Origin { get; set; }
    }

    [ApiController]
    [Route("/")]
    public class ProducerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProducerController> _logger;

        public ProducerController(ILogger<ProducerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                VirtualHost = "/"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null
            );

            var message = new Operation
            {
                Value1 = 1,
                Value2 = 2,
                OperationType = "+"
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(
                exchange: "", routingKey: "hello", basicProperties: null, body: body
                );


            return Ok();         
        }
    }
}
