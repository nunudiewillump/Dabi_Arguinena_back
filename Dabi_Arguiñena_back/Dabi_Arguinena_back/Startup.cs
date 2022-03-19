using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dabi_Arguinena_back.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ecommerce
{
    public class Startup
    {
        private readonly string _apiVersion = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(a => a.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects);

            services.AddDbContext<BlogContext>(options => options.UseSqlite("Data Source=blog_api.db"));

            services.AddSwaggerInApi(Configuration, _apiVersion);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseExceptionHandler("/error-local-development");// app.UseDeveloperExceptionPage(); 
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();

            app.UseSwaggerInApi(Configuration, _apiVersion);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


    public static class StartupExtensions
    {
        public static IServiceCollection AddSwaggerInApi(this IServiceCollection services, IConfiguration configuration, string apiVersion)
        {
            services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc(apiVersion, new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = $"Blog API San Valero ({apiVersion})",
                    Version = apiVersion,
                    Description = "API Rest of Blog to manage posts and categories",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Dabi Arguiñena",
                        Email = "masterdabi@gmail.com"
                    }
                });

            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerInApi(this IApplicationBuilder app, IConfiguration configuration, string apiVersion)
        {
            app.UseSwagger().UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", apiVersion);
                o.RoutePrefix = string.Empty;

            });

            return app;
        }
    }
}