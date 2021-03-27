using BPWA.Common.Services;
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
            services.AddScoped<IBusinessUnitsWebService, BusinessUnitsWebService>();
            services.AddScoped<ICitiesWebService, CitiesWebService>();
            services.AddScoped<ICompaniesWebService, CompaniesWebService>();
            services.AddScoped<ICountriesWebService, CountriesWebService>();
            services.AddScoped<ICurrenciesWebService, CurrenciesWebService>();
            services.AddScoped<ILanguagesWebService, LanguagesWebService>();
            services.AddScoped<ITicketsWebService, TicketsWebService>();

            #region Identity

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersWebService, UsersWebService>();
            
            services.AddScoped<IRolesWebService, RolesWebService>();
            services.AddScoped<IRolesService, RolesService>();

            services.AddScoped<CurrentUser, CurrentWebUser>();
            services.AddScoped<IPasswordGeneratorService, PasswordGeneratorService>();

            #endregion Identity

            #region Other

            services.AddScoped<IDropdownHelperService, DropdownHelperService>();
            services.AddScoped<IViewHelperService, ViewHelperService>();
            services.AddScoped<IEmailService, EmailService>();

            #endregion Other

            return services;
        }
    }
}
