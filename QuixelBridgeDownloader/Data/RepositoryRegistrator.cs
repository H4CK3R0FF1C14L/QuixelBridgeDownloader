using Microsoft.Extensions.DependencyInjection;
using QuixelBridgeDownloader.Data.Context;
using QuixelBridgeDownloader.Entities;
using QuixelBridgeDownloader.Interfaces.Base;

namespace QuixelBridgeDownloader.Data
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositoriesInDB(this IServiceCollection services) => services
            .AddTransient<IRepository<Item>, DbRepository<Item>>()
            .AddTransient<IRepository<Log>, DbRepository<Log>>()
        ;
    }
}
