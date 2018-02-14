using AutoMapper;
using ECommerce.Core.Data;
using ECommerce.Core.Data.Contexts;
using ECommerce.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ECommerce.WebAPI.Services
{
    public static class RegisterServicesExtension
    {
        /// <summary>
        /// Register services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {

            var settings = new Settings(configuration);

            services.AddSingleton<Settings>(settings);

            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
                {
                    // cfg.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ECommerceContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = settings.ValidIssuer,//_config["Tokens:Issuer"],
                        ValidAudience = settings.ValidAudience,// _config["Tokens:Audience"],
                        IssuerSigningKey = settings.IssuerSigningKey// new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    };

                });

            services.AddDbContext<ECommerceContext>(cfg =>
            {
                cfg.UseSqlServer(settings.SqlDbConnectionString);
            });


            services.AddAutoMapper();

            services.AddTransient<ECommerceSeeder>();

            services.AddScoped<IECommerceRepository, ECommerceRepository>();

            services.AddScoped<ITrackOrderService, TrackOrderService>();

            services.AddScoped<ITrackReviewService, TrackReviewService>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            

            services.AddMvc(opt =>
                {
                    if (env.IsProduction() && settings.DisableSSL)
                    {
                        opt.Filters.Add(new RequireHttpsAttribute());
                    }
                    opt.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    opt.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }
    }
}