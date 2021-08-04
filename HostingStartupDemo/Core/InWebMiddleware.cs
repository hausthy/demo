using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HostingStartupDemo.Core
{
    public class InWebMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public InWebMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<InWebMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"{nameof(InWebMiddleware)}");
            await _next(context);
        }
    }
}
