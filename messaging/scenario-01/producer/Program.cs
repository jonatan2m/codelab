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
                case "topic":
                    var topic = new Topic.TopicExample();
                    topic.RunConsumerSum();
                    topic.RunConsumerSumAll();
                    topic.RunConsumerSub();
                    topic.RunConsumerSubAll();
                    topic.RunConsumerAll();                    
                    break;
            }
            
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
