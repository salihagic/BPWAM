﻿using BPWA.Common.Security;
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

        #endregion 

        #region Items

        public bool ShowToggleCurrentCompanyItem() => _currentUser.CompanyIds().Any() || _currentUser.HasGodMode();
        public bool ShowToggleCurrentBusinessUnitItem() => _currentUser.BusinessUnitIds().Count > 1 || (_currentUser.BusinessUnitIds().Any() && _currentUser.CompanyIds().Any()) || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode();
        public bool ShowCompaniesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CompaniesManagement) || _currentUser.HasGodMode();
        public bool ShowRolesItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement) || _currentUser.HasGodMode()) && !_currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowUsersItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.UsersManagement) || _currentUser.HasGodMode()) && !_currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowCitiesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CitiesManagement) || _currentUser.HasGodMode();
        public bool ShowCountriesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CountriesManagement) || _currentUser.HasGodMode();
        public bool ShowCurrenciesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CurrenciesManagement) || _currentUser.HasGodMode();
        public bool ShowLanguagesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.LanguagesManagement) || _currentUser.HasGodMode();
        public bool ShowTranslationsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.TranslationsManagement) || _currentUser.HasGodMode();
        public bool ShowTicketsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.TicketsManagement) || _currentUser.HasGodMode();

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

        #endregion 

        #region Items

        public bool ShowBusinessUnitsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.BusinessUnitsManagement) || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode();
        public bool ShowCompanyRolesItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyRolesManagement) || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode()) && _currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowCompanyUsersItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyUsersManagement) || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode()) && _currentUser.CurrentCompanyId().HasValue && !_currentUser.CurrentBusinessUnitId().HasValue;

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

        #endregion 

        #region Items

        public bool ShowBusinessUnitRolesItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitRolesManagement) || _currentUser.HasBusinessUnitGodMode() || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode()) && _currentUser.CurrentBusinessUnitId().HasValue;
        public bool ShowBusinessUnitUsersItem() => (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitUsersManagement) || _currentUser.HasBusinessUnitGodMode() || _currentUser.HasCompanyGodMode() || _currentUser.HasGodMode()) && _currentUser.CurrentBusinessUnitId().HasValue;

        #endregion

        #endregion


        private CurrentUser _currentUser;

        public ViewHelperService(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
