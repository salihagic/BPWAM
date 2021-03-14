using System.Collections.Generic;
using System.Linq;

namespace BPWA.Common.Security
{
    public static class AppClaims
    {
        public static class Authorization
        {
            public static string CitiesManagement = nameof(CitiesManagement);
            public static string CountriesManagement = nameof(CountriesManagement);
            public static string CurrenciesManagement = nameof(CurrenciesManagement);
            public static string LanguagesManagement = nameof(LanguagesManagement);
         
            public static string RolesManagement = nameof(RolesManagement);
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
