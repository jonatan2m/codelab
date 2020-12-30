using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
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
    [Route("/producer")]
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
        [Route("direct")]
        public async Task<IActionResult> Direct(CancellationToken token)
        {
            System.Console.WriteLine("Producer - Direct");

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                VirtualHost = "/"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "direct-01", type: ExchangeType.Direct);

            string[] routings = { "info", "warning", "error" };

            while (true)
            {
                var ticks = DateTime.Now.Ticks;
                var routingPicked = string.Empty;
                if (ticks % 3 == 0) routingPicked = routings[1];
                else if (ticks % 2 == 0) routingPicked = routings[0];
                else routingPicked = routings[2];

                var message = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss");
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("direct-01", routingKey: routingPicked, null, body);

                await Task.Delay(100, token);
            }

            return Ok();
        }

        [HttpGet]
        [Route("fanout")]
        public async Task<IActionResult> Fanout(CancellationToken token)
        {
            System.Console.WriteLine("Producer - Fanout");
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                VirtualHost = "/"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "fanout-01", type: ExchangeType.Fanout);

            while (true)
            {
                var message = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss");
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "fanout-01", "", null, body);

                await Task.Delay(100, token);
            }
        }

        [HttpGet]
        [Route("queue")]
        public async Task<IActionResult> Queue(CancellationToken token)
        {
            System.Console.WriteLine("Producer - Queue");
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                VirtualHost = "/"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("fila-01", false, false, false, null);

            while (true)
            {
                var message = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss");
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", "fila-01", false, null, body);

                //Thread.Sleep(2000);
                await Task.Delay(100, token);
            }

            return Ok();
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
