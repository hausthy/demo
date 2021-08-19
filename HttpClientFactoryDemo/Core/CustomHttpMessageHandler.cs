using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientFactoryDemo.Core
{
    public class CustomHttpMessageHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public CustomHttpMessageHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var traceId = string.Empty;

            if (_accessor.HttpContext.Request.Headers.TryGetValue("traceId", out var tId))
            {
                traceId = tId.ToString();
                Console.WriteLine($"{traceId} from request {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.");
            }
            else
            {
                traceId = System.Guid.NewGuid().ToString("N");
                _accessor.HttpContext.Request.Headers.Add("traceId", new Microsoft.Extensions.Primitives.StringValues(traceId));
                Console.WriteLine($"{traceId} from generated {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.");
            }

            if (!request.Headers.Contains("trace-id"))
            {
                request.Headers.TryAddWithoutValidation("traceId", traceId);
            }

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
