using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Concurrent;
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
                client.Timeout = TimeSpan.FromSeconds(10);
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

        //����
        public IAsyncPolicy<HttpResponseMessage> FallbackPolicy(IServiceProvider serviceProvider, HttpRequestMessage request)
        {
            // ʹ�� Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage> ��ʽ��ȡPolicy ÿ�ζ��ᴴ��һ���µ�Policy
            policies.TryGetValue("FallbackPolicy", out IAsyncPolicy<HttpResponseMessage> policy);
            lock (policies)
            {
                if (policy == null)
                {
                    var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("FallbackPolicy");

                    policy = Policy<HttpResponseMessage>.Handle<Exception>().OrResult(hrm => hrm.StatusCode == HttpStatusCode.InternalServerError).FallbackAsync(new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { code = 1098, msg = "�����ݲ�����", data = new { } })),
                        StatusCode = HttpStatusCode.OK
                    }, async (b, ctx) =>
                    {
                        var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("FallbackPolicy");
                        logger?.LogError($"request error {b?.Result?.StatusCode} {b?.Exception?.Message}");
                        await Task.CompletedTask;
                    });

                    policies.TryAdd("FallbackPolicy", policy);
                }

                return policy;
            }
        }

        //�۶�
        public IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy(IServiceProvider serviceProvider, HttpRequestMessage request)
        {
            // ʹ�� Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage> ��ʽ��ȡPolicy ÿ�ζ��ᴴ��һ���µ�Policy
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
                             Console.WriteLine("��·����,�۶ϴ���.");

                         },
                         (ctx) =>
                         {
                             logger?.LogError($"service reset here ");
                             Console.WriteLine("�۶����ر���.");
                         },
                         () =>
                         {
                             //���۶�ʱ�䵽��֮�󴥷�
                             logger?.LogError($"service onHalfOpen here ");
                             Console.WriteLine("�۶�ʱ�䵽������뿪״̬");
                         });

                    policies.TryAdd("CircuitBreakerPolicy", policy);
                }

                return policy;
            }
        }

        //��ʱ
        public IAsyncPolicy<HttpResponseMessage> TimeOutPolicy(IServiceProvider serviceProvider, HttpRequestMessage request)
        {
            // ʹ�� Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage> ��ʽ��ȡPolicy ÿ�ζ��ᴴ��һ���µ�Policy 
            policies.TryGetValue("TimeOutPolicy", out IAsyncPolicy<HttpResponseMessage> policy);
            lock (policies)
            {
                if (policy == null)
                {
                    var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("TimeOutPolicy");

                    policy = Policy.TimeoutAsync<HttpResponseMessage>(20, (ctx, ts, tk, ex) =>
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
