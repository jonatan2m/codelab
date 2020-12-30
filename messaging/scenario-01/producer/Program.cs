using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Direct;
using Fanout;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using producer.Controllers;
using Queue;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace producer
{
    public class Program
    {
        static string routingKeyDiv2 = "div2";
        static string routingKeyDiv3 = "div3";
        static string routingKeyDiv5 = "div5";

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("choose: queue");
                return;
            }

            int consumerLength = int.Parse(args[1]);

            switch (args[0])
            {
                case "queue":
                    var queue = new QueueExample();
                    for (int i = 0; i < consumerLength; i++)
                        queue.RunConsumer();
                    break;
                case "fanout":
                    var fanout = new FanoutExample();
                    for (int i = 0; i < consumerLength; i++)
                        fanout.RunConsumer();
                    break;
                case "direct":
                    var direct = new DirectExample();
                    direct.RunConsumerInfo();
                    direct.RunConsumerWarning();
                    direct.RunConsumerError();
                    direct.RunConsumerAll();
                    break;
            }

            // var factory = new ConnectionFactory()
            // {
            //     HostName = "localhost",
            //     UserName = "rabbitmq",
            //     Password = "rabbitmq",
            //     VirtualHost = "/"
            // };
            // string queueName = "hello";
            // using var connection = factory.CreateConnection();            

            // BuildResultListener(connection);

            // BuildPublisher(connection, queueName, "Producer A");
            // BuildPublisher(connection, queueName, "Producer B");

            CreateHostBuilder(args).Build().Run();
        }

        public static void BuildResultListener(IConnection connection)
        {
            var channelDiv2 = connection.CreateModel();
            var channelDiv3 = connection.CreateModel();
            var channelDiv5 = connection.CreateModel();

            channelDiv2.ExchangeDeclare(exchange: "result",
                                    type: "direct");

            var queueDiv2 = channelDiv2.QueueDeclare().QueueName;
            var queueDiv3 = channelDiv3.QueueDeclare().QueueName;
            var queueDiv5 = channelDiv5.QueueDeclare().QueueName;

            channelDiv2.QueueBind(queueDiv2, exchange: "result", routingKey: routingKeyDiv2);
            channelDiv3.QueueBind(queueDiv3, exchange: "result", routingKey: routingKeyDiv3);
            channelDiv5.QueueBind(queueDiv5, exchange: "result", routingKey: routingKeyDiv5);

            BuildResultConsumer(channelDiv2, queueDiv2);
            BuildResultConsumer(channelDiv3, queueDiv3);
            BuildResultConsumer(channelDiv5, queueDiv5);
        }

        public static void BuildResultConsumer(IModel channel, string queueName)
        {

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ((model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine(" [x] Received '{0}':'{1}'",
                                  routingKey, message);
            });

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
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

                if (message.Contains(publisherName))
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
