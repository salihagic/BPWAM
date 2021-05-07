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
        public bool HasMultipleCompanies() => User.FindAll(AppClaims.Meta.HasMultipleCompanies) != null;
        public int? CurrentCompanyId()
        {
            var companyIdClaim = User.FindFirstValue(AppClaims.Meta.CurrentCompanyId);

            if (string.IsNullOrEmpty(companyIdClaim))
                return null;

            return int.Parse(companyIdClaim);
        }
        public string CurrentCompanyName() => User.FindFirstValue(AppClaims.Meta.CurrentCompanyName);
        public bool HasAuthorizationClaim(string claim) => User.Claims.Any(x => x.Type == AppClaimsHelper.Authorization.Type && x.Value == claim);
        public bool HasAdministrationAuthorizationClaim(string claim) => HasAuthorizationClaim(claim) || HasGodMode();
        public bool HasCompanyAuthorizationClaim(string claim) => HasAuthorizationClaim(claim) || HasCompanyGodMode() || HasGodMode();
        public bool HasGodMode() => HasAuthorizationClaim(AppClaims.Authorization.Administration.GodMode);
        public bool HasCompanyGodMode() => HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyGodMode);
        public List<string> Configuration() => User.FindAll(x => x.Type == AppClaimsHelper.Configuration.Type).Select(x => x.Value).ToList();

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
