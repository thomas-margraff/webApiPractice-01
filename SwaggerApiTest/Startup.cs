using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using DAL_SqlServer;
using DAL_SqlServer.Repository;
using System.Reflection;
using System.IO;

namespace SwaggerApiTest
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
            var allowedOrigins = Configuration.GetValue<string>("AllowedOrigins")?.Split(",") ?? new string[0];
            services.AddCors(options =>
            {
                options.AddPolicy("localAngularApp",
                    builder => builder
                                .WithOrigins(allowedOrigins)
                                .AllowAnyMethod()
                                .WithHeaders("Authentication")
                                .AllowCredentials());

                options.AddPolicy("PublicApi", builder => builder
                                .AllowAnyOrigin()
                                .WithMethods("Get")
                                .WithHeaders("Content-Type"));
            });

            services.AddDbContext<ntpContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("ntpConnectionString"));
            });

            services.AddScoped<IRepository, Repository<ntpContext>>();
            services.AddScoped<ICountriesRepository, CountriesRepository<ntpContext>>();
            services.AddScoped<IIndicatorDataRepository, IndicatorDataRepository<ntpContext>>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors("localAngularApp");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
