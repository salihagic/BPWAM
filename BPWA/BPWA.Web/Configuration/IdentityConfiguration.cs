using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Identity;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BPWA.Web.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddErrorDescriber<ApplicationIdentityErrorDescriber>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, AppClaimsPrincipalFactory>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/Authentication/AccessDenied";
                options.LoginPath = "/Authentication/Login";
                options.Cookie.HttpOnly = true;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            services.ConfigureApplicationCookie(x =>
            {
                x.ExpireTimeSpan = TimeSpan.FromDays(5);
                x.SlidingExpiration = true;
            });

            return services;
        }
    }
}
