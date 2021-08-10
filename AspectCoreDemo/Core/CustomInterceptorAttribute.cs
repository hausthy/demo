using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AspectCoreDemo.Core
{
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute
    {
        public CustomInterceptorAttribute()
        {
           
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var loggerFactory = context.ServiceProvider.GetService<ILoggerFactory>();

            var _logger = loggerFactory.CreateLogger<CustomInterceptorAttribute>();

            _logger.LogInformation($"{context.ServiceMethod.Name} Invoke Begin");

            await next(context);

            _logger.LogInformation($"{context.ServiceMethod.Name} Invoke End");
        }
    }
}
