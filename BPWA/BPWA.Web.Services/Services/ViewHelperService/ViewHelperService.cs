using BPWA.Common.Security;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BPWA.Web.Services.Services
{
    public class ViewHelperService : IViewHelperService
    {
        #region Administration

        #region Sections

        public bool ShowSystemSettingsSection() => new List<bool>{
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

        public bool ShowCitiesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CitiesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShowCompaniesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CompaniesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShowCountriesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CountriesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShowCurrenciesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CurrenciesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShowLanguagesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.LanguagesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShowRolesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.RolesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShowTicketsItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.TicketsManagement && x.Type == AppClaimsHelper.Authorization.Type);

        #endregion Items

        #endregion Administration

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;
        public ViewHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
