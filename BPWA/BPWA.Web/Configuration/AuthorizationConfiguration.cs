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

                options.AddPolicy(AppClaims.Authorization.Company.CompanyRolesManagement, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                                  AppClaims.Authorization.Administration.RolesManagement,
                                  AppClaims.Authorization.Company.CompanyRolesManagement));
                
                options.AddPolicy(AppClaims.Authorization.Company.ToggleBusinessUnit, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                                  AppClaims.Authorization.Administration.ToggleCompany,
                                  AppClaims.Authorization.Company.ToggleBusinessUnit));

                options.AddPolicy(AppClaims.Authorization.BusinessUnit.BusinessUnitRolesManagement, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                                  AppClaims.Authorization.Administration.RolesManagement,
                                  AppClaims.Authorization.Company.CompanyRolesManagement,
                                  AppClaims.Authorization.BusinessUnit.BusinessUnitRolesManagement));
            });

            return services;
        }
    }
}
