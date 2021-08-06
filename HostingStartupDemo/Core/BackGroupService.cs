using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostingStartupDemo.Core
{
    public class BackHostService : BackgroundService
    {
        public BackHostService()
        {

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("BackHostService.ExecuteAsync");

            return Task.CompletedTask;
        }
    }
}
