using BPWA.Common.Security;
using Microsoft.Extensions.DependencyInjection;

namespace BPWA.Web.Configuration
{
    public static class AuthorizationConfiguration
    {
        public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                foreach (var role in AppRolesHelper.All)
                    options.AddPolicy(role, policy => policy.RequireRole(role));

                options.AddPolicy(AppPolicies.CountriesManagement, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type, AppClaims.Authorization.CountriesManagement));
                options.AddPolicy(AppPolicies.CitiesManagement, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type, AppClaims.Authorization.CitiesManagement));
                options.AddPolicy(AppPolicies.CurrenciesManagement, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type, AppClaims.Authorization.CurrenciesManagement));
                options.AddPolicy(AppPolicies.LanguagesManagement, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type, AppClaims.Authorization.LanguagesManagement));

                options.AddPolicy(AppPolicies.RolesManagement, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type, AppClaims.Authorization.RolesManagement));
            });

            return services;
        }
    }
}
