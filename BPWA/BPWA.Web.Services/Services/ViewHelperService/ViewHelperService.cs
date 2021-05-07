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

        public bool ShowToggleCurrentCompanyItem() => _currentUser.HasMultipleCompanies();
        public bool ShowCompaniesItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.CompaniesManagement);
        public bool ShowRolesItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement) && !_currentUser.CurrentCompanyId().HasValue;
        public bool ShowUsersItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.UsersManagement) && !_currentUser.CurrentCompanyId().HasValue;
        public bool ShowCitiesItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.CitiesManagement);
        public bool ShowCountriesItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.CountriesManagement);
        public bool ShowCurrenciesItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.CurrenciesManagement);
        public bool ShowLanguagesItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.LanguagesManagement);
        public bool ShowLogsItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.LogsRead);
        public bool ShowTicketsItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.TicketsManagement);
        public bool ShowTranslationsItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.TranslationsManagement);
        public bool ShowNotificationsItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.NotificationsManagement) && !_currentUser.CurrentCompanyId().HasValue;
        public bool ShowGroupsItem() => HasAdministrationAuthorizationClaim(AppClaims.Authorization.Administration.GroupsManagement) && !_currentUser.CurrentCompanyId().HasValue;

        #endregion

        #endregion

        #region Company

        #region Sections

        public bool ShowCompanySection() => new List<bool>{
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

        public bool ShowCompanyRolesItem() => HasCompanyAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement);
        public bool ShowCompanyUsersItem() => HasCompanyAuthorizationClaim(AppClaims.Authorization.Administration.UsersManagement);
        public bool ShowCompanyNotificationsItem() => HasCompanyAuthorizationClaim(AppClaims.Authorization.Administration.NotificationsManagement);
        public bool ShowCompanyGroupsItem() => HasCompanyAuthorizationClaim(AppClaims.Authorization.Administration.GroupsManagement);

        #endregion

        #endregion

        #region Helpers

        bool HasAdministrationAuthorizationClaim(string claim) => _currentUser.HasAdministrationAuthorizationClaim(claim) || _currentUser.HasGodMode();
        bool HasCompanyAuthorizationClaim(string claim) => _currentUser.HasCompanyAuthorizationClaim(claim) && _currentUser.CurrentCompanyId().HasValue;

        private ICurrentUser _currentUser;

        public ViewHelperService(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        #endregion
    }
}
