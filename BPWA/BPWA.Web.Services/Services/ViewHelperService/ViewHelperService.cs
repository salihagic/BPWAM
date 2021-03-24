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
            ShowAuthSection(),
            ShowGeolocationsSection(),
        }.Any(x => x);

        public bool ShowAuthSection() => new List<bool>{
            ShowCompaniesItem(),
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

        public bool ShowToggleCompanyDropdown() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.ToggleCompany);
        public bool ShowCitiesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CitiesManagement);
        public bool ShowCompaniesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CompaniesManagement);
        public bool ShowCountriesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CountriesManagement);
        public bool ShowCurrenciesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CurrenciesManagement);
        public bool ShowLanguagesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.LanguagesManagement);
        public bool ShowRolesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement);
        public bool ShowTicketsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.TicketsManagement);

        #endregion Items

        #endregion Administration

        #region Company

        #region Sections

        public bool ShowCompanySection() => new List<bool>{
            ShowBusinessUnitsItem(),
        }.Any(x => x);

        #endregion Sections

        #region Items

        public bool ShowBusinessUnitsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.BusinessUnitsManagement);

        #endregion Items

        #endregion Company

        private CurrentUser _currentUser;

        public ViewHelperService(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
