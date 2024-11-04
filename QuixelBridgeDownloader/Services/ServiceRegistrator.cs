using Microsoft.Extensions.DependencyInjection;
using QuixelBridgeDownloader.Interfaces.Services;
using QuixelBridgeDownloader.Services.Database;

namespace QuixelBridgeDownloader.Services
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddScoped<HttpClient, HttpClient>()
            .AddTransient<IArchiveService, ArchiveService>()
            .AddTransient<IItemService, ItemService>()
            .AddTransient<ILogService, LogService>()
            .AddTransient<IMenuService, MenuService>()
            .AddTransient<IQuixelService, QuixelService>()
            .AddTransient<ITelegramService, TelegramService>()
        ;
    }
}
