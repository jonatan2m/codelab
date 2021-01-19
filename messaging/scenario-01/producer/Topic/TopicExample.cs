using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Topic
{
    public class TopicExample
    {
        private readonly IConnection connection;

        public TopicExample()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                VirtualHost = "/"
            };

            connection = factory.CreateConnection();
        }

        public void RunConsumerSum()
        {
            Task.Run(() =>
            {
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("topic-01", type: ExchangeType.Topic);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "topic-01", "sum.info");
                channel.QueueBind(queueName, "topic-01", "sum.warning");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"sum.info|warning {message}");

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });
                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(500);
                }
            });
        }

         public void RunConsumerSub()
        {
            Task.Run(() =>
            {
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("topic-01", type: ExchangeType.Topic);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "topic-01", "sub.info");
                channel.QueueBind(queueName, "topic-01", "sub.warning");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"sub.info|warning {message}");

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });
                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(500);
                }
            });
        }

        public void RunConsumerSumAll()
        {
            Task.Run(() =>
            {
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("topic-01", type: ExchangeType.Topic);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "topic-01", "sum.*");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"sum.* {message}");

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });
                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(500);
                }
            });
        }

        public void RunConsumerSubAll()
        {
            Task.Run(() =>
            {
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("topic-01", type: ExchangeType.Topic);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "topic-01", "sub.*");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"sub.* {message}");

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });
                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(500);
                }
            });
        }

         public void RunConsumerAll()
        {
            Task.Run(() =>
            {
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("topic-01", type: ExchangeType.Topic);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, "topic-01", "#");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    System.Console.WriteLine($"ALL {message}");

                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var t = message;
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                });
                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Thread.Sleep(500);
                }
            });
        }
    }
}