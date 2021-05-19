using BPWA.Common.Services;
using BPWA.DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BPWA.Api.Configuration
{
    //Keep the order of services alphabetical(as they are sorted in the folder)
    public static class AppServicesConfiguration
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService, AccountsService>()
                    .AddScoped<ICompaniesService, CompaniesService>()
                    .AddScoped<ICompanyActivityStatusLogsService, CompanyActivityStatusLogsService>()
                    .AddScoped<ILogsService, LogsService>()
                    .AddScoped<INotificationsService, NotificationsService>()
                    .AddScoped<ITranslationsService, TranslationsService>()
            #region Identity
                    .AddScoped<IUsersService, UsersService>()
                    .AddScoped<IRolesService, RolesService>()
            #endregion 
            #region Helpers
                    .AddScoped<ICurrentBaseCompany, CurrentBaseCompany>()
                    .AddScoped<ICurrentCompany, CurrentCompany>()
                    .AddScoped<ICurrentTimezone, CurrentTimezone>()
                    .AddScoped<ICurrentUser, CurrentUser>()
                    .AddScoped<IPasswordGeneratorService, PasswordGeneratorService>()
                    .AddScoped<IEmailService, EmailService>();
            #endregion 

            return services;
        }
    }
}
