using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web3_1.CustomMiddleware;
using Web3_1.FluentValidationExamples;
using Web3_1.Handlers;

namespace Web3_1
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
            services.AddScoped<EventDispacher>();
            services.AddScoped<MovieRepository>();
            //services.AddMediatR(typeof(Startup));
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            
            //Inject all Validators in specific Assembly which was the same this extension method is used
            //services.AddValidatorsFromAssemblyContaining<EventViewModelValidator>();
            
            //services.AddSwaggerGen(setup =>
            //{                
            //    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            //    {
            //        Version = "v1",
            //        Title = "Example Swagger",
            //        Description = "This is a silly test",
            //        License = new Microsoft.OpenApi.Models.OpenApiLicense
            //        {
            //            Name = "Whatever",
            //            Url = new Uri("https://projetomatrix.com.br")
            //        },
            //        TermsOfService = new Uri("https://projetomatrix.com.br"),
            //        Contact = new Microsoft.OpenApi.Models.OpenApiContact
            //        {
            //            Email = "jonatan@projetomatrix.com.br",
            //            Url = new Uri("https://projetomatrix.com.br"),
            //            Name = "Jonatan Machado",                        
            //        }
            //    });

                
                

            //    /* Enable XML documentation
            //        <PropertyGroup>
            //            <GenerateDocumentationFile>true</GenerateDocumentationFile>  
            //            <NoWarn>$(NoWarn);1591</NoWarn>
            //        </PropertyGroup>
            //    */
            //    //enabling XML Documentation (summary comment)
            //    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    setup.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI(setup =>
                {
                    setup.InjectStylesheet("/swagger-ui/custom.css");                    
                });
            }            

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<BlockEventCodeMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
