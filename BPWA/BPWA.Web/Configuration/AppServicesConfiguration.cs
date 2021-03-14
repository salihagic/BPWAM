using BPWA.DAL.Services;
using BPWA.Web.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BPWA.Web.Configuration
{
    //Keep the order of services alphabetical(as they are sorted in the folder)
    public static class AppServicesConfiguration
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            services.AddScoped<ICitiesWebService, CitiesWebService>();
            services.AddScoped<ICountriesWebService, CountriesWebService>();
            services.AddScoped<ICurrenciesWebService, CurrenciesWebService>();
            services.AddScoped<ILanguagesWebService, LanguagesWebService>();


            #region Identity

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersWebService, UsersWebService>();
            services.AddScoped<ILoggedUserService, LoggedUserWebService>();
            
            services.AddScoped<IRolesWebService, RolesWebService>();
            services.AddScoped<IRolesService, RolesService>();

            services.AddScoped<IAppClaimsService, AppClaimsService>();

            #endregion Identity

            #region Other

            services.AddScoped<IViewHelperService, ViewHelperService>();

            #endregion Other

            return services;
        }
    }
}
