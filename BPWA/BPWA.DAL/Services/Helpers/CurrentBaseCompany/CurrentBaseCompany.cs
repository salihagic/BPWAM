using BPWA.Common.Enumerations;
using BPWA.Common.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace BPWA.DAL.Services
{
    public class CurrentBaseCompany : ICurrentBaseCompany
    {
        public int? Id()
        {
            var companyIdClaim = User?.FindFirstValue(AppClaims.Meta.BaseCompanyId);

            if (string.IsNullOrEmpty(companyIdClaim))
                return null;

            return int.Parse(companyIdClaim);
        }

        public bool IsGuest() => AccountType() == Common.Enumerations.AccountType.Guest;

        public AccountType? AccountType()
        {
            var companyAccountType = User?.FindFirstValue(AppClaims.Meta.BaseCompanyAccountType);

            if (string.IsNullOrEmpty(companyAccountType))
                return null;

            return Enum.Parse<AccountType>(companyAccountType);
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public CurrentBaseCompany(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
