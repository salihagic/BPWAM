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
            ShouldShowRolesSection(),
        }.Any();
        public bool ShouldShowRolesSection() => User.Claims.Any(x => x.Value == AppClaims.Authorization.RolesManagement && x.Type == AppClaimsHelper.Authorization.Type);

        public bool ShouldShowGeolocationsSection() => new List<bool>{
            ShouldShowCitiesSection(),
            ShouldShowCountriesSection(),
            ShouldShowCurrenciesSection(),
            ShouldShowLanguagesSection(),
        }.Any(x => x);     
        public bool ShouldShowCitiesSection() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CitiesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShouldShowCountriesSection() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CountriesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShouldShowCurrenciesSection() => User.Claims.Any(x => x.Value == AppClaims.Authorization.CurrenciesManagement && x.Type == AppClaimsHelper.Authorization.Type);
        public bool ShouldShowLanguagesSection() => User.Claims.Any(x => x.Value == AppClaims.Authorization.LanguagesManagement && x.Type == AppClaimsHelper.Authorization.Type);

        public bool ShouldShowGeneralSystemSettingsSection() => new List<bool>{
            ShouldShowTicketsSection(),
        }.Any(x => x);
        public bool ShouldShowTicketsSection() => User.Claims.Any(x => x.Value == AppClaims.Authorization.TicketsManagement && x.Type == AppClaimsHelper.Authorization.Type);

        #endregion System settings section

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;
        public ViewHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
