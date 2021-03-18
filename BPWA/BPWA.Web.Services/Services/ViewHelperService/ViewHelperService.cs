using BPWA.Common.Security;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BPWA.Web.Services.Services
{
    public class ViewHelperService : IViewHelperService
    {
        #region System settings section

        public bool ShouldShowSystemSettingsSection() => new List<bool>{
            ShouldShowAuthSection(),
            ShouldShowGeolocationsSection(),
        }.Any(x => x);

        public bool ShouldShowAuthSection() => new List<bool>{
            ShouldShowCompaniesItem(),
            ShouldShowRolesItem(),
        }.Any(x => x);
        public bool ShouldShowCompaniesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CompaniesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShouldShowRolesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.RolesManagement && x.Type == AppClaimsHelper.Authorization.Type);

        public bool ShouldShowGeolocationsSection() => new List<bool>{
            ShouldShowCitiesItem(),
            ShouldShowCountriesItem(),
            ShouldShowCurrenciesItem(),
            ShouldShowLanguagesItem(),
        }.Any(x => x);     
        public bool ShouldShowCitiesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CitiesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShouldShowCountriesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CountriesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShouldShowCurrenciesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CurrenciesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShouldShowLanguagesItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.LanguagesManagement && x.Type == AppClaimsHelper.Authorization.Type);

        public bool ShouldShowGeneralSystemSettingsSection() => new List<bool>{
            ShouldShowTicketsItem(),
        }.Any(x => x);
        public bool ShouldShowTicketsItem() => User.Claims.Any(x => x.Value == AppClaims.Authorization.TicketsManagement && x.Type == AppClaimsHelper.Authorization.Type);

        #endregion System settings section

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;
        public ViewHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
