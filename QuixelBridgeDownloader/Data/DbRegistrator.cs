using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuixelBridgeDownloader.Data.Context;

namespace QuixelBridgeDownloader.Data
{
    public static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) => services
            .AddDbContext<ApplicationContext>(opt =>
            {
                var type = configuration["Type"];
                switch (type)
                {
                    case null: throw new InvalidOperationException("Can't connect to Database");
                    default: throw new InvalidOperationException($"Unknown Database Type: {type}");

                    case "MSSQL":
                        opt.UseSqlServer(configuration.GetConnectionString(type));
                        break;

                    case "SQLite":
                        opt.UseSqlite(configuration.GetConnectionString(type));
                        break;

                    case "InMemory":
                        opt.UseInMemoryDatabase("DB.db");
                        break;

                    case "MySQL":
                        opt.UseMySQL(configuration.GetConnectionString(type)!);
                        break;
                }
            })
            .AddTransient<DbInitializer>()
            .AddRepositoriesInDB()
        ;
    }
}
