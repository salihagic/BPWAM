using System.Collections.Generic;
using System.Linq;

namespace BPWA.Common.Security
{
    public static class AppClaims
    {
        public static class Authorization
        {
            public static class Administration
            {
                public const string CitiesManagement = nameof(CitiesManagement);
                public const string CompaniesManagement = nameof(CompaniesManagement);
                public const string CountriesManagement = nameof(CountriesManagement);
                public const string CurrenciesManagement = nameof(CurrenciesManagement);
                public const string LanguagesManagement = nameof(LanguagesManagement);
                public const string RolesManagement = nameof(RolesManagement);
                public const string UsersManagement = nameof(UsersManagement);
                public const string TicketsManagement = nameof(TicketsManagement);
                public const string ToggleCompany = nameof(ToggleCompany);
            }

            public static class Company
            {
                public const string CompanyRolesManagement = nameof(CompanyRolesManagement);
                public const string BusinessUnitsManagement = nameof(BusinessUnitsManagement);
                public const string ToggleBusinessUnit = nameof(ToggleBusinessUnit);
            }

            public static class BusinessUnit
            {
                public const string BusinessUnitRolesManagement = nameof(BusinessUnitRolesManagement);
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
            public static string CompanyId = nameof(CompanyId);
            public static string CompanyName = nameof(CompanyName);
            public static string BusinessUnitId = nameof(BusinessUnitId);
            public static string BusinessUnitName = nameof(BusinessUnitName);
        }
    }

    public static class AppClaimsHelper
    {
        public static class Authorization
        {
            public static string Type => nameof(Authorization);
            public static List<string> All {
                get
                {
                    var all = new List<string>();

                    all.AddRange(Administration.All);
                    all.AddRange(Company.All);
                    all.AddRange(BusinessUnit.All);

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

            public static class BusinessUnit
            {
                public static List<string> All => typeof(AppClaims.Authorization.BusinessUnit).GetFields().Select(x => x.Name).ToList();
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
