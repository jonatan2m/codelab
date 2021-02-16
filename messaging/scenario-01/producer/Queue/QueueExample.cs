using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Queue
{
    public class QueueExample
    {
        public void RunConsumer()
        {
            Task.Run(() =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "rabbitmq",
                    Password = "rabbitmq",
                    VirtualHost = "/"
                };

                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.QueueDeclare("fila-01", false, false, false, null);

                channel.BasicQos(0, 1, false);

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

                channel.BasicConsume("fila-01", false, consumer);

                while (true)
                {            
                    Thread.Sleep(1000);
                }
            });
        }
    }
}