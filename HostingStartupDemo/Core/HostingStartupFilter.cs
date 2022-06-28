using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace HostingStartupDemo.Core
{
    public class HostingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                Console.WriteLine("HostingStartupFilter.Configure1");
                app.UseMiddleware<InWebMiddleware>();
                next(app);
                Console.WriteLine("HostingStartupFilter.Configure2");
            };
        }
    }
}
