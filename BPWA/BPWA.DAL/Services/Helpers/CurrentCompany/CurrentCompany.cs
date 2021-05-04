using BPWA.Common.Security;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BPWA.DAL.Services
{
    public class CurrentCompany : ICurrentCompany
    {
        public int? Id()
        {
            var companyIdClaim = User?.FindFirstValue(AppClaims.Meta.CurrentCompanyId);

            if (string.IsNullOrEmpty(companyIdClaim))
                return null;

            return int.Parse(companyIdClaim);
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public CurrentCompany(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
