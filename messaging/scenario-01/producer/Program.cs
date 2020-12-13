using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using producer.Controllers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                VirtualHost = "/"
            };
            string queueName = "hello";
            using var connection = factory.CreateConnection();            

            BuildPublisher(connection, queueName, "Producer A");
            BuildPublisher(connection, queueName, "Producer B");

            CreateHostBuilder(args).Build().Run();
        }

        //Just to pratice. It doesn't make sense in real scenario
        public static void BuildLogListener(IConnection connection, string publisherName)
        {
            var logs = connection.CreateModel();
            logs.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            var someQueue = logs.QueueDeclare().QueueName;

            logs.QueueBind(queue: someQueue, exchange: "logs", routingKey: "");
            var consumer = new EventingBasicConsumer(logs);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if(message.Contains(publisherName))
                    System.Console.WriteLine($"{publisherName}.{message}");
            };

            logs.BasicConsume(queue: someQueue, autoAck: true, consumer: consumer);
        }

        public static void BuildPublisher(IConnection connection, string queue, string publisherName)
        {
            Task.Run(() =>
            {
                BuildLogListener(connection, publisherName);

                IModel channel = connection.CreateModel();
                int count = 0;

                channel.QueueDeclare(
                    queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null
                );

                Random r = new Random();

                while (true)
                {
                    var message = new Operation
                    {
                        Value1 = r.Next(),
                        Value2 = r.Next(),
                        OperationType = "+",
                        Origin = publisherName
                    };

                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(
                        exchange: "", routingKey: queue, basicProperties: properties, body: body);

                    System.Console.WriteLine($"{publisherName} - Sent {count++}");

                    System.Threading.Thread.Sleep(1000);
                }
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
