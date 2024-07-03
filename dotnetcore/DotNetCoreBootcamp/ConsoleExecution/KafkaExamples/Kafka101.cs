using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleExecution.KafkaExamples
{
    public class Kafka101
    {
        public static void Run()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "", //usually of the form cell-1.streaming.[region code].oci.oraclecloud.com:9092
                //SslCaLocation = @"<path\to\root\ca\certificate\*.pem>",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                
                SaslUsername = "",
                SaslPassword = ""
            };

            Produce("email_worker-notification-sent", config); // use the name of the stream you created


            

        }

        static void Produce(string topic, ClientConfig config)
        {
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {   
                producer.Produce(topic, new Message<string, string> { Key = "1", Value = "valor 1" });                
                var result = producer.Flush(TimeSpan.FromSeconds(3));

                int numProduced = 0;
                int numMessages = 10;
                for (int i = 0; i < numMessages; ++i)
                {
                    var key = "messageKey" + i;
                    var val = "messageVal" + i;

                    Console.WriteLine($"Producing record: {key} {val}");

                    producer.Produce(topic, new Message<string, string> { Key = key, Value = val },
                        (deliveryReport) =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
                                numProduced += 1;
                            }
                        });
                }

                producer.Flush(TimeSpan.FromSeconds(10));

                Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
            }
        }
    }
}
