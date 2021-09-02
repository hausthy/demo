using Microsoft.Extensions.Configuration;

namespace ConfigurationDemo.Config
{
    public class CustomConfigSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomConfigProvider();
        }
    }
}
