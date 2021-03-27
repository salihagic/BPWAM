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
            services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"))
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<DatabaseSettings>>().Value)
                    .Configure<EmailSettings>(configuration.GetSection("EmailSettings"))
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<EmailSettings>>().Value)
                    .Configure<IdentityOptions>(configuration.GetSection("IdentityOptions"))
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<IdentityOptions>>().Value)
                    .Configure<PasswordOptions>(configuration.GetSection("IdentityOptions:Password"))
                    .AddSingleton(resolver => resolver.GetRequiredService<IOptions<PasswordOptions>>().Value);

            return services;
        }
    }
}
