using BPWA.Common.Configuration;
using BPWA.DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BPWA.Web.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
        {
            var databaseSettings = services.BuildServiceProvider().GetRequiredService<DatabaseSettings>();

            if (databaseSettings.UsingPostgresDatabase)
            {
                services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(databaseSettings.ConnectionString));
            }
            else if (databaseSettings.UsingMicrosoftSQLServerDatabase)
            {
                services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(databaseSettings.ConnectionString));
            }

            return services;
        }
    }
}
