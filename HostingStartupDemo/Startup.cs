using HostingStartupDemo.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace HostingStartupDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Console.WriteLine("Startup ");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Startup.ConfigureServices ");

            services.AddControllers();

            services.AddHealthChecks();

            services.AddHostedService<BackHostService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Startup.Configure1");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (content, next) =>
           {
               await next();
           });

            //app.Run(async (context) =>
            //{
            //    if (context.Request.Path == "/health")
            //    {
            //        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("ok"));
            //    }
            //});

            app.UseHealthChecks("/health");

            //app.UseHealthChecks("/health", new HealthCheckOptions
            //{
            //    AllowCachingResponses = true,
            //    ResponseWriter = async (context, healthReport) => await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("ok"))
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Console.WriteLine("Startup.Configure2");
        }
    }
}
