using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Diagnostics;

//通过HostingStartup指定要启动的类型
[assembly: HostingStartup(typeof(HostingStartupDemo.Core.HostingStartupInWeb))]
namespace HostingStartupDemo.Core
{
    public class HostingStartupInWeb : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            //可以添加配置
            builder.ConfigureAppConfiguration(config =>
            {
                //模拟添加一个一个内存配置
                var datas = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("ServiceName", "HostingStartupDemo")
                };
                config.AddInMemoryCollection(datas);
            });

            //可以添加ConfigureServices
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, HostingStartupFilter>();
            });

            ////可以添加Configure
           // builder.Configure(app =>
           //{
           //    app.UseMiddleware<InWebMiddleware>();
           //});
        }
    }
}
