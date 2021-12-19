using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Web3_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Host created");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();                    
                    logging.AddConsole();
                    //Serilog Configuration
                    var config = new ConfigurationBuilder()
                        .AddJsonFile($"appsettings.Development.json")
                        .Build();

                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config)
                        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Minute)
                        .CreateLogger();
                    //Requires installing Serilog.Extensions.Logging;
                    logging.AddSerilog(logger, dispose: true);
                })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>();
                });
    }
}
