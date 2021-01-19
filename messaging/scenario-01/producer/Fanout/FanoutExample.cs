using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fanout
{
    public class FanoutExample
    {
        public void RunConsumer()
        {
            Task.Run(() =>
            {
                System.Console.WriteLine("Consumer - Fanout");
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

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "fanout-01", "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine(message);

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });

                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(1000);
                }
            });
        }
    }
}