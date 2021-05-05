using BPWA.Common.Security;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

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
                var commonClaims = new List<string>
                {
                    AppClaims.Authorization.Administration.UsersManagement,
                    AppClaims.Authorization.Administration.RolesManagement,
                    AppClaims.Authorization.Administration.NotificationsManagement,
                    AppClaims.Authorization.Administration.GroupsManagement,
                };

                foreach (var claim in AppClaimsHelper.Authorization.Administration.All)
                {
                    if (commonClaims.Contains(claim))
                    {
                        options.AddPolicy(claim, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                        claim,
                        AppClaims.Authorization.Company.CompanyGodMode,
                        AppClaims.Authorization.Administration.GodMode));
                    }
                    else
                    {
                        options.AddPolicy(claim, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                            claim,
                            AppClaims.Authorization.Administration.GodMode));
                    }
                }
                foreach (var claim in AppClaimsHelper.Authorization.Company.All)
                {
                    options.AddPolicy(claim, policy => policy.RequireClaim(AppClaimsHelper.Authorization.Type,
                        claim,
                        AppClaims.Authorization.Company.CompanyGodMode,
                        AppClaims.Authorization.Administration.GodMode));
                }
            });

            return services;
        }
    }
}
