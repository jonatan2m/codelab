using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Direct
{
    public class DirectExample
    {
        private readonly IModel channel;

        public DirectExample()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                VirtualHost = "/"
            };

            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void RunConsumerInfo()
        {
            Task.Run(() =>
            {
                channel.ExchangeDeclare(exchange: "direct-01", type: ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "direct-01", "info");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"info-{message} ({Thread.CurrentThread.ManagedThreadId})");

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });

                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(10);
                }
            });
        }

        public void RunConsumerWarning()
        {
            Task.Run(() =>
            {
                channel.ExchangeDeclare(exchange: "direct-01", type: ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "direct-01", "warning");

                channel.BasicQos(0, 2, true);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"warning-{message} ({Thread.CurrentThread.ManagedThreadId})");

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });

                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(10);
                }
            });
        }

         public void RunConsumerError()
        {
            Task.Run(() =>
            {
                channel.ExchangeDeclare(exchange: "direct-01", type: ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "direct-01", "error");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"error-{message} ({Thread.CurrentThread.ManagedThreadId})");

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });

                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(10);
                }
            });
        }

        public void RunConsumerAll()
        {
            Task.Run(() =>
            {
                channel.ExchangeDeclare(exchange: "direct-01", type: ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "direct-01", "info");
                channel.QueueBind(queueName, "direct-01", "warning");
                channel.QueueBind(queueName, "direct-01", "error");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"ALL-{message} ({Thread.CurrentThread.ManagedThreadId})");

                    channel.BasicAck(ea.DeliveryTag, false);
                });

                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(10);
                }
            });
        }
    }
}