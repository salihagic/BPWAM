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
                foreach (var claim in AppClaimsHelper.Authorization.All)
                    options.AddPolicy(claim, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type, 
                                      claim));
               
                options.AddPolicy(AppClaims.Authorization.BusinessUnitsUsersManagement, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                                  AppClaims.Authorization.CompaniesUsersManagement,
                                  AppClaims.Authorization.BusinessUnitsUsersManagement));
            });

            return services;
        }
    }
}
