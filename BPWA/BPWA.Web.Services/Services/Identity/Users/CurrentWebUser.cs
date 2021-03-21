using BPWA.Common.Security;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BPWA.Web.Services.Services
{
    public class CurrentWebUser : CurrentUser
    {
        public string Id() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string UserName() => User.FindFirstValue(ClaimTypes.Name);
        public string FirstName() => User.FindFirstValue(ClaimTypes.GivenName);
        public string LastName() => User.FindFirstValue(ClaimTypes.Surname);
        public string FullName() => $"{FirstName()} {LastName()}";
        public string TimezoneId() => User.FindFirstValue(AppClaims.Meta.TimezoneId);
        public int? CompanyId() 
        {
            var companyIdClaim = User.FindFirstValue(AppClaims.Meta.CompanyId);

            if (string.IsNullOrEmpty(companyIdClaim))
                return null;

            return int.Parse(companyIdClaim);
        }
        public string CompanyName() => User.FindFirstValue(AppClaims.Meta.CompanyName);
        public int? BusinessUnitId()
        {
            var companyIdClaim = User.FindFirstValue(AppClaims.Meta.BusinessUnitId);

            if (string.IsNullOrEmpty(companyIdClaim))
                return null;

            return int.Parse(companyIdClaim);
        }
        public string BusinessUnitName() => User.FindFirstValue(AppClaims.Meta.BusinessUnitName);
        public bool HasAuthorizationClaim(string claim) => User.Claims.Any(x => x.Type == AppClaimsHelper.Authorization.Type && x.Value == claim);
        public List<string> Configuration() => User.FindAll(x => x.Type == AppClaimsHelper.Configuration.Type).Select(x => x.Value).ToList();

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        public CurrentWebUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
