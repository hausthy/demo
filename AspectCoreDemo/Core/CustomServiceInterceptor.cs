using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;

namespace AspectCoreDemo.Core
{
    public class CustomServiceInterceptor : AbstractInterceptor
    {
        private readonly ILogger _logger;

        public CustomServiceInterceptor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CustomServiceInterceptor>();
        }


        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            _logger.LogInformation($"{context.ServiceMethod.Name} Invoke Begin");

            await next(context);

            _logger.LogInformation($"{context.ServiceMethod.Name} Invoke End");
        }
    }
}
