using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class ConsumerHostedService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
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

        channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            try
            {
                if(ea.DeliveryTag % 5 == 0) throw new ArgumentException();

                var body = ea.Body.ToArray();
                var operation = JsonSerializer.Deserialize<Operation>(Encoding.UTF8.GetString(body));

                System.Console.WriteLine(ea.DeliveryTag);
                System.Console.WriteLine($"{ea.DeliveryTag}.{operation.Execute()}");                
                
                Task.Delay(500);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        channel.BasicConsume(queue: "hello", autoAck: false, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            Task.Delay(1000, stoppingToken);
        }

        return Task.CompletedTask;
    }
}