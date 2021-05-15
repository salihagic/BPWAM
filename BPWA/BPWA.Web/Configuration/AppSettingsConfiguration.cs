using BPWA.Common.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BPWA.Web.Configuration
{
    public static class AppSettingsConfiguration
    {
        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings")) 
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppSettings>>().Value)
                    .Configure<BackgroundServicesSettings>(configuration.GetSection("BackgroundServicesSettings")) 
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<BackgroundServicesSettings>>().Value)
                    .Configure<CacheSettings>(configuration.GetSection("CacheSettings")) 
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<CacheSettings>>().Value)
                    .Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings")) 
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<DatabaseSettings>>().Value)
                    .Configure<EmailSettings>(configuration.GetSection("EmailSettings"))
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<EmailSettings>>().Value)
                    .Configure<RouteSettings>(configuration.GetSection("RouteSettings"))
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<RouteSettings>>().Value)
                    .Configure<IdentityOptions>(configuration.GetSection("IdentityOptions"))
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<IdentityOptions>>().Value)
                    .Configure<PasswordOptions>(configuration.GetSection("IdentityOptions:Password"))
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<PasswordOptions>>().Value);

            return services;
        }
    }
}
