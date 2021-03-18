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
        public string GetId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string GetUserName() => User.FindFirstValue(ClaimTypes.Name);
        public string GetFirstName() => User.FindFirstValue(ClaimTypes.GivenName);
        public string GetLastName() => User.FindFirstValue(ClaimTypes.Surname);
        public string GetFullName() => $"{GetFirstName()} {GetLastName()}"; 
        public string GetTimezoneId() => User.FindFirstValue(AppClaims.Meta.TimezoneId);
        public bool HasClaim(string claim) => User.Claims.Any(x => x.Type == AppClaimsHelper.Authorization.Type && x.Value == claim);
        public List<string> GetConfiguration() => User.FindAll(x => x.Type == AppClaimsHelper.Configuration.Type).Select(x => x.Value).ToList();

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        public CurrentWebUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
