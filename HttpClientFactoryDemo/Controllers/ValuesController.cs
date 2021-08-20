using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactoryDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ValuesController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ValuesController(IHttpClientFactory clientFactory, ILogger<ValuesController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }


        [HttpGet]
        public async Task<string> Get()
        {
            var client = _clientFactory.CreateClient("demo1");

            //var traceId = Guid.NewGuid().ToString("N");
            //Console.WriteLine(traceId);
            //client.DefaultRequestHeaders.TryAddWithoutValidation("traceId", traceId);

            var res = await client.GetAsync("http://localhost:5000/api/values/demo1");
            var str = await res.Content.ReadAsStringAsync();
            return str;
        }


        [HttpGet("demo1")]
        public async Task<string> GetDemo1()
        {
            var client = _clientFactory.CreateClient("demo2");
            var res = await client.GetAsync("http://localhost:5000/api/values/demo2");
            var str = await res.Content.ReadAsStringAsync();
            return str;
        }

        [HttpGet("demo2")]
        public async Task<string> GetDemo2()
        {
            var client = _clientFactory.CreateClient("demo3");
            var res = await client.GetAsync("http://localhost:5000/api/values/demo3");
            var str = await res.Content.ReadAsStringAsync();
            return str;
        }

        [HttpGet("demo3")]
        public async Task<string> GetDemo3()
        {
            await Task.CompletedTask;

            return "demo3";
        }
    }
}