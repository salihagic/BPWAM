﻿using BPWA.Common.Extensions;
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

        public bool ShowToggleCompanyItem()
        {
            var hasGodMode = _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);

            var hasMultipleCompanies = _currentUser.CompanyIds().Count > 1;

            return hasGodMode || hasMultipleCompanies;
        }
        public bool ShowCitiesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CitiesManagement) || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);
        public bool ShowCompaniesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CompaniesManagement) || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);
        public bool ShowCountriesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CountriesManagement) || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);
        public bool ShowCurrenciesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.CurrenciesManagement) || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);
        public bool ShowLanguagesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.LanguagesManagement) || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);
        public bool ShowRolesItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement) || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);
        public bool ShowTicketsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.TicketsManagement) || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);

        #endregion Items

        #endregion Administration

        #region Company

        #region Sections

        public bool ShowCompanySection() => new List<bool>{
            ShowBusinessUnitsItem(),
        }.Any(x => x);

        #endregion Sections

        #region Items

        public bool ShowBusinessUnitsItem() => _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.BusinessUnitsManagement) || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode);

        #endregion Items

        #endregion Company

        private CurrentUser _currentUser;

        public ViewHelperService(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
