using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Buffers;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;

namespace WebApplication3_1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                var cancellationToken = context.RequestAborted;

                var reader = context.Request.BodyReader;
                var resultReader = await context.Request.BodyReader.ReadAsync(cancellationToken);
                var buffer = resultReader.Buffer;
                
                var s = context.Request.PathBase;

                var found = buffer.FirstSpan.IndexOf(tag);
                if (found != -1) await next();

                //await context.Response.StartAsync(cancellationToken);

                //context.Response.StatusCode = 200;

                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //Precisa mapear a rota somente em um lugar. não pode estar aqui e no controller
                //endpoints.MapPost("/weatherforecast", async context =>
                //{
                //    await Task.Yield();
                //});
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        public static ReadOnlySpan<byte> tag => new[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t', (byte)'e', };
    }
}
