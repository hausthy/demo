using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientFactoryPolly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<OrderController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public OrderController(ILogger<OrderController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("polly");
        }

        [HttpGet]
        public async Task<string> Get()
        {
            try
            {
                var result = await _httpClient.GetAsync("http://127.0.0.1:5001/order/polly");

                var strRes = await result.Content.ReadAsStringAsync();

                return strRes;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Get");
            }

            return "";
        }

        [HttpGet("polly")]
        public string GetPolly()
        {
            //Thread.Sleep(TimeSpan.FromSeconds(5));

            throw new ArgumentNullException();

            return "GetPolly";
        }
    }
}
