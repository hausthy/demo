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

                var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

                var _logger = loggerFactory.CreateLogger<HostingStartupFilter>();

                _logger.LogInformation("HostingStartupFilter");

                app.UseMiddleware<InWebMiddleware>();
                next(app);
            };
        }
    }
}
