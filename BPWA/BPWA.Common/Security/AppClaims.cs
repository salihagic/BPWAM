using System.Collections.Generic;
using System.Linq;

namespace BPWA.Common.Security
{
    public static class AppClaims
    {
        public static class Authorization
        {
            public class Administration
            {
                public const string GodMode = nameof(GodMode);
                public const string CitiesManagement = nameof(CitiesManagement);
                public const string CountriesManagement = nameof(CountriesManagement);
                public const string CurrenciesManagement = nameof(CurrenciesManagement);
                public const string LanguagesManagement = nameof(LanguagesManagement);
                public const string LogsRead = nameof(LogsRead);
                public const string TranslationsManagement = nameof(TranslationsManagement);
                public const string ConfigurationManagement = nameof(ConfigurationManagement);
            }

            public static class Company
            {
                public const string CompanyGodMode = nameof(CompanyGodMode);
                public const string CompaniesManagement = nameof(CompaniesManagement);
                public const string RolesManagement = nameof(RolesManagement);
                public const string UsersManagement = nameof(UsersManagement);
                public const string NotificationsManagement = nameof(NotificationsManagement);
                public const string GroupsManagement = nameof(GroupsManagement);
                public const string TicketsManagement = nameof(TicketsManagement);
            }
        }

        public static class Configuration
        {
            //Not yet used, just an example
            public static string HasMobileAppAccess = nameof(HasMobileAppAccess);
        }

        public static class Meta
        {
            public static string TimezoneId = nameof(TimezoneId);
            public static string CurrentCompanyId = nameof(CurrentCompanyId);
            public static string CurrentCompanyName = nameof(CurrentCompanyName);
            public static string BaseCompanyId = nameof(BaseCompanyId);
            public static string HasMultipleCompanies = nameof(HasMultipleCompanies);
        }
    }

    public static class AppClaimsHelper
    {
        public static class Authorization
        {
            public static string Type => nameof(Authorization);
            public static List<string> All
            {
                get
                {
                    var all = new List<string>();

                    all.AddRange(Administration.All);
                    all.AddRange(Company.All);

                    return all;
                }
            }

            public static class Administration
            {
                public static List<string> All => typeof(AppClaims.Authorization.Administration).GetFields().Select(x => x.Name).ToList();
            }

            public static class Company
            {
                public static List<string> All => typeof(AppClaims.Authorization.Company).GetFields().Select(x => x.Name).ToList();
            }
        }

        public static class Configuration
        {
            public static string Type => nameof(AppClaims.Configuration);
        }

        public static class Meta
        {
            public static string Type => nameof(AppClaims.Meta);
        }
    }
}
