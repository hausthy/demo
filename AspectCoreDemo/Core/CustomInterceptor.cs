using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;

namespace AspectCoreDemo.Core
{
    public class CustomInterceptor : AbstractInterceptor
    {

        [FromServiceContext]
        public ILogger<CustomInterceptor> Logger { get; set; }


        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Logger.LogInformation($"{context.ServiceMethod.Name} Invoke Begin");

            await next(context);

            Logger.LogInformation($"{context.ServiceMethod.Name} Invoke End");
        }
    }
}
