using System.IO;
using Microsoft.Extensions.Configuration;

namespace CleanVidly.IntegrationTest.Helpers
{
    public class ConfigurationSingleton
    {
        private static IConfigurationRoot configuration;
        private ConfigurationSingleton() { }
        public static IConfigurationRoot GetConfiguration()
        {
            if (configuration is null)
                configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Path.GetFullPath("../../../")))
                .AddJsonFile("appsettings.Test.json")
                .AddEnvironmentVariables()
                .Build();


            return configuration;
        }
    }
}