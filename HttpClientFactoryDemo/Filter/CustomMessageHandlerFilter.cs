using HttpClientFactoryDemo.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Http;
using System;

namespace HttpClientFactoryDemo.Filter
{
    public class CustomHttpMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly IHttpContextAccessor _accessor;

        public CustomHttpMessageHandlerBuilderFilter(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {

            return builder =>
            {
                next(builder);

                builder.AdditionalHandlers.Add(new CustomHttpMessageHandler(_accessor));
            };
        }
    }
}
