using BPWA.Common.Configuration;
using BPWA.DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BPWA.Web.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = services.BuildServiceProvider().GetRequiredService<DatabaseSettings>();
 
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(databaseSettings.ConnectionString));

            return services;
        }
    }
}
