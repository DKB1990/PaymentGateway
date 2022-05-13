using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.API.IntegrationTest
{
    public class ApiFixture: IDisposable
    {
        public readonly WebApplicationFactory<Startup> factory;

        public ApiFixture()
        {
            var configuration = GetIConfigurationRoot();
            factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddConfiguration(configuration);
                });
            });
        }

        public void Dispose() => factory.Dispose();

        private static IConfigurationRoot GetIConfigurationRoot() => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
    }
}