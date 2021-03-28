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
                var administrationClaims = AppClaimsHelper.Authorization.Administration.All;
                var companyClaims = AppClaimsHelper.Authorization.Company.All;
                var businessUnitClaims = AppClaimsHelper.Authorization.BusinessUnit.All;

                foreach (var claim in AppClaimsHelper.Authorization.All)
                {
                    if (administrationClaims.Contains(claim))
                    {
                        options.AddPolicy(claim, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type, 
                            claim, 
                            AppClaims.Authorization.Administration.GodMode));
                    }
                    else if (companyClaims.Contains(claim))
                    {
                        options.AddPolicy(claim, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                            claim, 
                            AppClaims.Authorization.Company.CompanyGodMode,
                            AppClaims.Authorization.Administration.GodMode));
                    }
                    else if (businessUnitClaims.Contains(claim))
                    {
                        options.AddPolicy(claim, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                            claim,
                            AppClaims.Authorization.BusinessUnit.BusinessUnitGodMode,
                            AppClaims.Authorization.Company.CompanyGodMode,
                            AppClaims.Authorization.Administration.GodMode));
                    }
                }
            });

            return services;
        }
    }
}
