using BPWA.Common.Security;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BPWA.DAL.Services
{
    public class CurrentUser : ICurrentUser
    {
        public string Id() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string UserName() => User.FindFirstValue(ClaimTypes.Name);
        public string FirstName() => User.FindFirstValue(ClaimTypes.GivenName);
        public string LastName() => User.FindFirstValue(ClaimTypes.Surname);
        public string FullName() => $"{FirstName()} {LastName()}";
        public string TimezoneId() => User.FindFirstValue(AppClaims.Meta.TimezoneId);
        public List<int> CompanyIds() => User.FindAll(AppClaims.Authorization.CompanyIds).Select(x => int.Parse(x.Value)).ToList();
        public int? CurrentCompanyId()
        {
            var companyIdClaim = User.FindFirstValue(AppClaims.Meta.CurrentCompanyId);

            if (string.IsNullOrEmpty(companyIdClaim))
                return null;

            return int.Parse(companyIdClaim);
        }
        public string CurrentCompanyName() => User.FindFirstValue(AppClaims.Meta.CurrentCompanyName);
        public List<int> BusinessUnitIds() => User.FindAll(AppClaims.Authorization.BusinessUnitIds).Select(x => int.Parse(x.Value)).ToList();
        public int? CurrentBusinessUnitId()
        {
            var companyIdClaim = User.FindFirstValue(AppClaims.Meta.CurrentBusinessUnitId);

            if (string.IsNullOrEmpty(companyIdClaim))
                return null;

            return int.Parse(companyIdClaim);
        }
        public string CurrentBusinessUnitName() => User.FindFirstValue(AppClaims.Meta.CurrentBusinessUnitName);
        public bool HasAuthorizationClaim(string claim) => User.Claims.Any(x => x.Type == AppClaimsHelper.Authorization.Type && x.Value == claim) || HasGodMode();
        public bool HasGodMode() => User.Claims.Any(x => x.Type == AppClaimsHelper.Authorization.Type && x.Value == AppClaims.Authorization.Administration.GodMode);
        public bool HasCompanyGodMode() => HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyGodMode);
        public bool HasBusinessUnitGodMode() => HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitGodMode);
        public List<string> Configuration() => User.FindAll(x => x.Type == AppClaimsHelper.Configuration.Type).Select(x => x.Value).ToList();

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
