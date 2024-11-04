using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuixelBridgeDownloader.Data;
using QuixelBridgeDownloader.Interfaces.Services;
using QuixelBridgeDownloader.Services;
using System.Globalization;

namespace QuixelBridgeDownloader
{
    internal class Program
    {
        #region Properties

        #region Private Properties

        #region HOST

        private static IHost? __Host;

        public static IHost Host => __Host ??= Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder(Environment.GetCommandLineArgs())
            .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("appsettings.json", true, true))
            .ConfigureServices((host, services) => services
                .AddDatabase(host.Configuration.GetSection("Database"))
                .AddServices()
                .AddLogging(c => c.ClearProviders()) // Clear Logging
                )
            .Build();

        public static IServiceProvider Services => Host.Services;

        #endregion

        private static IMenuService? _MenuService;

        #endregion

        #endregion

        #region Private Methods

        private static async Task Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US", false);

            StartHost(Host);

            AsyncServiceScope scope = Services.CreateAsyncScope();
            _MenuService = scope.ServiceProvider.GetRequiredService<IMenuService>();

            await _MenuService.CreateMenuAsync();

            StopExit(Host);
        }

        private static void StartHost(IHost host)
        {
            using (IServiceScope scope = Services.CreateScope())
                scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync().Wait();

            host.Start();
        }

        private static void StopExit(IHost host)
        {
            host.StopAsync().Wait();
        }

        #endregion
    }
}
