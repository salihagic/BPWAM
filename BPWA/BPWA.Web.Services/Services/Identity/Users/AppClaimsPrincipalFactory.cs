using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        private DatabaseContext _databaseContext;

        public AppClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            DatabaseContext databaseContext
            ) : base(userManager, roleManager, optionsAccessor) 
        {
            _databaseContext = databaseContext;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);
            var claims = new List<Claim>();

            if (user.FirstName != null)
                claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            if (user.LastName != null)
                claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            if (user.TimezoneId != null)
                claims.Add(new Claim(AppClaims.Meta.TimezoneId, user.TimezoneId));
            if (user.CompanyId != null) 
            {
                var company = await _databaseContext.Companies.FirstOrDefaultAsync(x => x.Id == user.CompanyId);

                claims.Add(new Claim(AppClaims.Meta.CompanyId, user.CompanyId.ToString()));
                claims.Add(new Claim(AppClaims.Meta.CompanyName, company.Name));
            }
            if (user.BusinessUnitId != null)
            {
                var businessUnit = await _databaseContext.BusinessUnits.FirstOrDefaultAsync(x => x.Id == user.BusinessUnitId);

                claims.Add(new Claim(AppClaims.Meta.BusinessUnitId, user.BusinessUnitId.ToString()));
                claims.Add(new Claim(AppClaims.Meta.BusinessUnitName, businessUnit.Name));
            }

            (principal.Identity as ClaimsIdentity).AddClaims(claims);

            return principal;
        }
    }
}
