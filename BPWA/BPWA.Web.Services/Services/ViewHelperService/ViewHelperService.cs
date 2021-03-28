using BPWA.Common.Security;
using BPWA.DAL.Services;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.Web.Services.Services
{
    public class ViewHelperService : IViewHelperService
    {
        #region Administration

        #region Sections

        public bool ShowAdministrationSection() => new List<bool>{
            ShowCompaniesItem(),
            ShowAuthSection(),
            ShowGeolocationsSection(),
        }.Any(x => x);

        public bool ShowAuthSection() => new List<bool>{
            ShowUsersItem(),
            ShowRolesItem(),
        }.Any(x => x);

        public bool ShowGeolocationsSection() => new List<bool>{
            ShowCitiesItem(),
            ShowCountriesItem(),
            ShowCurrenciesItem(),
            ShowLanguagesItem(),
        }.Any(x => x);

        #endregion Sections

        #region Items

        public bool ShowToggleCurrentCompanyItem() => _currentUser.CompanyIds().Count > 1 || _currentUser.HasGodMode();
        public bool ShowToggleCurrentBusinessUnitItem() => _currentUser.BusinessUnitIds().Count > 1 || (_currentUser.BusinessUnitIds().Any() && _currentUser.CompanyIds().Any()) || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode();
        public bool ShowCompaniesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CompaniesManagement) || _currentUser.HasGodMode();
        public bool ShowUsersItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement) || _currentUser.HasGodMode();
        public bool ShowRolesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement) || _currentUser.HasGodMode();
        public bool ShowCitiesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CitiesManagement) || _currentUser.HasGodMode();
        public bool ShowCountriesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CountriesManagement) || _currentUser.HasGodMode();
        public bool ShowCurrenciesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CurrenciesManagement) || _currentUser.HasGodMode();
        public bool ShowLanguagesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.LanguagesManagement) || _currentUser.HasGodMode();
        public bool ShowTicketsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.TicketsManagement) || _currentUser.HasGodMode();

        #endregion Items

        #endregion Administration

        #region Company

        #region Sections

        public bool ShowCompanySection() => new List<bool>{
            ShowBusinessUnitsItem(),
        }.Any(x => x);

        #endregion Sections

        #region Items

        public bool ShowBusinessUnitsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.BusinessUnitsManagement) || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode();

        #endregion Items

        #endregion Company

        private CurrentUser _currentUser;

        public ViewHelperService(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
