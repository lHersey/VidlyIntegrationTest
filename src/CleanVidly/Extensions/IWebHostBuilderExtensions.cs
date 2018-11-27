using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CleanVidly.Extensions
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder UseSentryOnProduction(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureAppConfiguration(RegisterSentryConfig(webHostBuilder));
        }



        private static Action<WebHostBuilderContext, IConfigurationBuilder> RegisterSentryConfig(IWebHostBuilder webHostBuilder)
        {
            return (hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                if (env.IsProduction())
                {
                    var sentryKey = hostingContext.Configuration["vidly_SentryKey"];

                    if (String.IsNullOrEmpty(sentryKey)) throw new InvalidOperationException("Error: The 'vidly_SentryKey' environment variable is not set, set this envitonment variable to the sentry dns");

                    webHostBuilder.UseSentry();
                }
            };

        }
    }
}