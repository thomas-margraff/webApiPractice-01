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
using DAL_SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using DAL_SqlServer.Repository;

namespace webApiPractice_01
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
                    builder => builder.WithOrigins(allowedOrigins).AllowAnyMethod().WithHeaders("Authentication").AllowCredentials());
                options.AddPolicy("PublicApi", builder => builder.AllowAnyOrigin().WithMethods("Get").WithHeaders("Content-Type"));
            });

            services.AddDbContext<ntpContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("ntpConnectionString"));
            }
            );

            services.AddScoped<IRepository, Repository<ntpContext>>();
            services.AddScoped<ICountriesRepository, CountriesRepository<ntpContext>>();
            services.AddScoped<IIndicatorDataRepository, IndicatorDataRepository<ntpContext>>();

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

            app.UseRouting();
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
