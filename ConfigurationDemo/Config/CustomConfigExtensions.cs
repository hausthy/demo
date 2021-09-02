using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationDemo.Config
{
    public static class CustomConfigExtensions
    {
        public static IConfigurationBuilder AddCustomConfig(this IConfigurationBuilder builder)
        {
            var source = new CustomConfigSource();

            return builder.Add(source);
        }
    }
}
