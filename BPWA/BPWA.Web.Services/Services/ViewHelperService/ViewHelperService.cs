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
            ShowLogsItem(),
            ShowTicketsItem(),
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
            ShowTranslationsItem(),
        }.Any(x => x);
        
        public bool ShowNotificationsSection() => new List<bool>{
            ShowNotificationsItem(),
            ShowGroupsItem(),
        }.Any(x => x);

        #endregion 

        #region Items

        public bool ShowToggleCurrentCompanyItem() => _currentUser.CompanyIds().Any() || _currentUser.HasGodMode();
        public bool ShowToggleCurrentBusinessUnitItem() => _currentUser.BusinessUnitIds().Count > 1 || (_currentUser.BusinessUnitIds().Any() && _currentUser.CompanyIds().Any()) || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode();
        public bool ShowCompaniesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CompaniesManagement) || _currentUser.HasGodMode();
        public bool ShowRolesItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement) || _currentUser.HasGodMode()) && !_currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowUsersItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.UsersManagement) || _currentUser.HasGodMode()) && !_currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowCitiesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CitiesManagement);
        public bool ShowCountriesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CountriesManagement);
        public bool ShowCurrenciesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CurrenciesManagement);
        public bool ShowLanguagesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.LanguagesManagement);
        public bool ShowLogsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.LogsRead);
        public bool ShowTicketsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.TicketsManagement);
        public bool ShowTranslationsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.TranslationsManagement);
        public bool ShowNotificationsItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.NotificationsManagement) || _currentUser.HasGodMode()) && !_currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowGroupsItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.GroupsManagement) || _currentUser.HasGodMode()) && !_currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;

        #endregion

        #endregion

        #region Company

        #region Sections

        public bool ShowCompanySection() => new List<bool>{
            ShowBusinessUnitsItem(),
            ShowCompanyAuthSection(),
        }.Any(x => x);

        public bool ShowCompanyAuthSection() => new List<bool>{
            ShowCompanyRolesItem(),
            ShowCompanyUsersItem(),
        }.Any(x => x);

        public bool ShowCompanyNotificationsSection() => new List<bool>{
            ShowCompanyNotificationsItem(),
            ShowCompanyGroupsItem(),
        }.Any(x => x);

        #endregion 

        #region Items

        public bool ShowBusinessUnitsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.BusinessUnitsManagement) || _currentUser.HasCompanyGodMode();
        public bool ShowCompanyRolesItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyRolesManagement) || _currentUser.HasCompanyGodMode()) && _currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowCompanyUsersItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyUsersManagement) || _currentUser.HasCompanyGodMode()) && _currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowCompanyNotificationsItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyNotificationsManagement) || _currentUser.HasCompanyGodMode()) && _currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowCompanyGroupsItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyGroupsManagement) || _currentUser.HasCompanyGodMode()) && _currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;

        #endregion

        #endregion

        #region Business unit

        #region Sections

        public bool ShowBusinessUnitSection() => new List<bool>{
            ShowBusinessUnitAuthSection(),
        }.Any(x => x);

        public bool ShowBusinessUnitAuthSection() => new List<bool>{
            ShowBusinessUnitRolesItem(),
            ShowBusinessUnitUsersItem(),
        }.Any(x => x);

        public bool ShowBusinessUnitNotificationsSection() => new List<bool>{
            ShowBusinessUnitNotificationsItem(),
            ShowBusinessUnitGroupsItem(),
        }.Any(x => x);

        #endregion 

        #region Items

        public bool ShowBusinessUnitRolesItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitRolesManagement) || _currentUser.HasBusinessUnitGodMode()) && _currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowBusinessUnitUsersItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitUsersManagement) || _currentUser.HasBusinessUnitGodMode()) && _currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowBusinessUnitNotificationsItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitNotificationsManagement) || _currentUser.HasBusinessUnitGodMode()) && _currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowBusinessUnitGroupsItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitGroupsManagement) || _currentUser.HasBusinessUnitGodMode()) && _currentUser.CurrentBusinessUnitId().HasValue;

        #endregion

        #endregion

        private ICurrentUser _currentUser;

        public ViewHelperService(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
