using IslandIs.Skjalaveita.Api.Services;
using IslandIs.Skjalaveita.DocumentindexCLI.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using System;
using System.Threading.Tasks;

namespace IslandIs.Skjalaveita.DocumentindexCLI
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Create service collection and configure our services
            var services = new ServiceCollection();
            ConfigureServices(services);

            // Generate a provider
            var serviceProvider = services.BuildServiceProvider();

            // Kick off our actual code
            serviceProvider.GetService<ConsoleApplication>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddLogging(configure => configure
                .AddConsole()
                .AddSerilog()
            );

            // build configuration
            var configuration = new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json", true, true)
                      .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<DocumentindexServiceSettings>(configuration.GetSection("DocumentindexServiceSettings"));
            serviceCollection.Configure<ConsoleApplicationSettings>(configuration.GetSection("ConsoleApplicationSettings"));

            // add services 
            serviceCollection.AddSingleton<DocumentindexService>();

            // add app
            serviceCollection.AddSingleton<ConsoleApplication>();
        }

    }
}
