using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Timeout;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactoryPolly
{
    public class Startup
    {
        private static ConcurrentDictionary<string, IAsyncPolicy<HttpResponseMessage>> policies = new ConcurrentDictionary<string, IAsyncPolicy<HttpResponseMessage>>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpClient("polly", client =>
            {
                client.BaseAddress = new Uri("http://127.0.0.1:5001");
                client.Timeout = TimeSpan.FromSeconds(2);
            })
             .AddPolicyHandler(FallbackPolicy)
             .AddPolicyHandler(CircuitBreakerPolicy)
             .AddPolicyHandler(TimeOutPolicy);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //降级
        public IAsyncPolicy<HttpResponseMessage> FallbackPolicy(IServiceProvider serviceProvider, HttpRequestMessage request)
        {
            // 使用 Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage> 方式获取Policy 每次都会创建一个新的Policy
            policies.TryGetValue("FallbackPolicy", out IAsyncPolicy<HttpResponseMessage> policy);
            lock (policies)
            {
                if (policy == null)
                {
                    var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("FallbackPolicy");

                    policy = Policy<HttpResponseMessage>.Handle<Exception>().FallbackAsync(new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { code = 1098, msg = "服务暂不可用", data = new { } })),
                        StatusCode = HttpStatusCode.OK
                    }, async (b, ctx) =>
                    {
                        var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("FallbackPolicy");
                        logger?.LogError("request error", b.Exception.Message);
                        await Task.CompletedTask;
                    });

                    policies.TryAdd("FallbackPolicy", policy);
                }

                return policy;
            }
        }

        //熔断
        public IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy(IServiceProvider serviceProvider, HttpRequestMessage request)
        {
            // 使用 Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage> 方式获取Policy 每次都会创建一个新的Policy
            policies.TryGetValue("CircuitBreakerPolicy", out IAsyncPolicy<HttpResponseMessage> policy);
            lock (policies)
            {
                if (policy == null)
                {
                    var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("CircuitBreakerPolicy");

                    policy = Policy<HttpResponseMessage>.Handle<Exception>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(10),
                         (de, ts, ctx) =>
                         {
                             logger?.LogError($"service breaked here {ts.TotalSeconds}s", de.Exception.Message);
                             Console.WriteLine("断路器打开,熔断触发.");

                         },
                         (ctx) =>
                         {
                             logger?.LogError($"service reset here ");
                             Console.WriteLine("熔断器关闭了.");
                         },
                         () =>
                         {
                             //在熔断时间到了之后触发
                             logger?.LogError($"service onHalfOpen here ");
                             Console.WriteLine("熔断时间到，进入半开状态");
                         });

                    policies.TryAdd("CircuitBreakerPolicy", policy);
                }

                return policy;
            }
        }

        //超时
        public IAsyncPolicy<HttpResponseMessage> TimeOutPolicy(IServiceProvider serviceProvider, HttpRequestMessage request)
        {
            // 使用 Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage> 方式获取Policy 每次都会创建一个新的Policy 
            policies.TryGetValue("TimeOutPolicy", out IAsyncPolicy<HttpResponseMessage> policy);
            lock (policies)
            {
                if (policy == null)
                {
                    var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("TimeOutPolicy");

                    policy = Policy.TimeoutAsync<HttpResponseMessage>(1, (ctx, ts, tk, ex) =>
                   {
                       logger?.LogError("TimeOutPolicy", ex.Message);
                       return Task.CompletedTask;

                   });

                    policies.TryAdd("TimeOutPolicy", policy);
                }

                return policy;
            }

        }
    }
}
