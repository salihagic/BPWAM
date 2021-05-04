using BPWA.Common.Security;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BPWA.DAL.Services
{
    public class CurrentBusinessUnit : ICurrentBusinessUnit
    {
        public int? Id()
        {
            var companyIdClaim = User?.FindFirstValue(AppClaims.Meta.CurrentBusinessUnitId);

            if (string.IsNullOrEmpty(companyIdClaim))
                return null;

            return int.Parse(companyIdClaim);
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public CurrentBusinessUnit(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
