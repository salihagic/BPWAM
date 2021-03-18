using System.Collections.Generic;
using System.Linq;

namespace BPWA.Common.Security
{
    public static class AppClaims
    {
        public static class Authorization
        {
            #region Administration

            public const string CitiesManagement = nameof(CitiesManagement);
            public const string CompaniesManagement = nameof(CompaniesManagement);
            public const string CountriesManagement = nameof(CountriesManagement);
            public const string CurrenciesManagement = nameof(CurrenciesManagement);
            public const string LanguagesManagement = nameof(LanguagesManagement);
            public const string RolesManagement = nameof(RolesManagement);
            public const string UsersManagement = nameof(UsersManagement);
            public const string TicketsManagement = nameof(TicketsManagement);

            #endregion

            #region Companies

            public const string CompaniesUsersManagement = nameof(CompaniesUsersManagement);

            #endregion

            #region Business units

            public const string BusinessUnitsUsersManagement = nameof(BusinessUnitsUsersManagement);

            #endregion
        }

        public static class Configuration
        {
            //Not yet used, just an example
            public static string HasMobileAppAccess = nameof(HasMobileAppAccess);
        }

        public static class Meta
        {
            public static string TimezoneId = nameof(TimezoneId);
        }
    }

    public static class AppClaimsHelper
    {
        public static class Authorization
        {
            public static string Type => nameof(AppClaims.Authorization);
            public static List<string> All => typeof(AppClaims.Authorization).GetFields().Select(x => x.Name).ToList();
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
