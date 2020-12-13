using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace consumer
{
    public class Program
    {
        static HashSet<ulong> controls;
        public static void Main(string[] args)
        {
            controls = new HashSet<ulong>();

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                VirtualHost = "/"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            //define limit to unack messages 
            //channel.BasicQos(0, 5, false);

            var logs = BuildExchange(connection);

            BuildConsumer(channel, logs, $"Consumer A", 10000);
            BuildConsumer(channel, logs, $"Consumer B", 5000);
            BuildConsumer(channel, logs, $"Consumer C", 3000);
            BuildConsumer(channel, logs, $"Consumer D", 1000);
            BuildConsumer(channel, logs, $"Consumer E", 1000);
            BuildConsumer(channel, logs, $"Consumer F", 1000);
            BuildConsumer(channel, logs, $"Consumer G", 1000);

            CreateHostBuilder(args).Build().Run();
        }

        public static IModel BuildExchange(IConnection connection)
        {
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            return channel;
        }

        public static void BuildConsumer(IModel channel, IModel logs, string consumerName, int sleepSeconds)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                Operation operation = JsonSerializer.Deserialize<Operation>(Encoding.UTF8.GetString(body));
                try
                {
                    if (ea.DeliveryTag % 5 == 0 && controls.Add(ea.DeliveryTag))
                    {
                        throw new ArgumentException();
                    }

                    System.Console.WriteLine($"{consumerName}.{ea.DeliveryTag} = {operation.Execute()}");

                    await Task.Delay(sleepSeconds);
                    //Thread.Sleep(sleepSeconds);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {                    
                    body = Encoding.UTF8.GetBytes(string.Concat(operation.Origin,ex.Message));
                    logs.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body);

                    System.Console.WriteLine("Error");
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            channel.BasicConsume(queue: "hello", autoAck: false, consumer: consumer);
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
