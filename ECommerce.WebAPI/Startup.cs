using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Core.Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ECommerce.Core.Entities;
using Microsoft.IdentityModel.Tokens;
using ECommerce.WebAPI.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;

namespace ECommerce.WebAPI
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private string _basePath;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(cfg =>
            {
                cfg.AddPolicy("AnyOperation", bldr =>
                {
                    bldr.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });

                cfg.AddPolicy("AnyGET", bldr =>
                {
                    bldr.AllowAnyHeader()
                        .WithMethods("GET")
                        .AllowAnyOrigin();
                });
            });

            services.RegisterServices(Configuration, _env);

            _basePath = AppContext.BaseDirectory;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Demo Order API",
                    Version = "v1",
                    Contact = new Contact { Name = "David Chen", Email = "xchen178@hotmail.com" }
                });
                // Set the comments path for the Swagger JSON and UI
                var xmlPath = Path.Combine(_basePath, "ECommerce.WebAPI.xml");
                c.IncludeXmlComments(xmlPath);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole();
                loggerFactory.AddDebug();

                // Seed the database
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<ECommerceSeeder>();
                    seeder.Seed().Wait();
                }

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            ILogger logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500,
                                exceptionHandlerFeature.Error,
                                exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");

                    });
                });
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API V1");
            });

            //app.UseHttpCacheHeaders();

            app.UseStaticFiles();

            app.UseCors("AnyOperation");

            app.UseAuthentication();

            app.UseMvc();


            app.Run(async context =>
            {
                context.Response.Redirect("swagger");
            });
        }
    }
}
