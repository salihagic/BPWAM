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
            services.AddScoped<IAccountsWebService, AccountsWebService>()
                    .AddScoped<IAccountsService, AccountsService>()
                    .AddScoped<ICitiesWebService, CitiesWebService>()
                    .AddScoped<ICompaniesWebService, CompaniesWebService>()
                    .AddScoped<ICompaniesService, CompaniesService>()
                    .AddScoped<ICompanyActivityStatusLogsService, CompanyActivityStatusLogsService>()
                    .AddScoped<IConfigurationWebService, ConfigurationWebService>()
                    .AddScoped<ICountriesWebService, CountriesWebService>()
                    .AddScoped<ICurrenciesWebService, CurrenciesWebService>()
                    .AddScoped<IGroupsWebService, GroupsWebService>()
                    .AddScoped<ILanguagesWebService, LanguagesWebService>()
                    .AddScoped<ILogsService, LogsService>()
                    .AddScoped<INotificationsWebService, NotificationsWebService>()
                    .AddScoped<ITicketsWebService, TicketsWebService>()
                    .AddScoped<ITranslationsService, TranslationsService>()
                    .AddScoped<ITranslationsWebService, TranslationsWebService>()
            #region Identity
                    .AddScoped<IUsersService, UsersService>()
                    .AddScoped<IUsersWebService, UsersWebService>()
                    .AddScoped<IRolesWebService, RolesWebService>()
                    .AddScoped<IRolesService, RolesService>()
            #endregion 
            #region Helpers
                    .AddScoped<ICurrentBaseCompany, CurrentBaseCompany>()
                    .AddScoped<ICurrentCompany, CurrentCompany>()
                    .AddScoped<ICurrentTimezone, CurrentTimezone>()
                    .AddScoped<ICurrentUser, CurrentUser>()
                    .AddScoped<IDropdownHelperService, DropdownHelperService>()
                    .AddScoped<IViewHelperService, ViewHelperService>()
                    .AddScoped<IPasswordGeneratorService, PasswordGeneratorService>()
                    .AddScoped<IEmailService, EmailService>();
            #endregion 

            return services;
        }
    }
}
