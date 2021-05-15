using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
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
            var claimsIdentity = (principal.Identity as ClaimsIdentity);
            var claims = new List<Claim>();

            await AddBasicInfo(user, claims);
            await AddRoles(user, claims, claimsIdentity);
            await AddCurrentCompanyId(user, claims);

            claimsIdentity.AddClaims(claims);

            return principal;
        }

        async Task AddBasicInfo(User user, List<Claim> claims)
        {
            if (user.FirstName != null)
                claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            if (user.LastName != null)
                claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            if (user.TimezoneId != null)
                claims.Add(new Claim(AppClaims.Meta.TimezoneId, user.TimezoneId));
        }

        async Task AddRoles(User user, List<Claim> claims, ClaimsIdentity claimsIdentity)
        {
            //Remove automatically generated claims and roles
            //so they can be added manually
            foreach (var claim in claimsIdentity.Claims.ToList())
                if (claim.Type == ClaimTypes.Role || claim.Type == AppClaimsHelper.Authorization.Type)
                    claimsIdentity.RemoveClaim(claim);

            //User is allowed to everything from top to bottom
            //If the user has CompanyGodMode on the parent company
            //he should be able to do anything in child companies
            var roles = await _databaseContext.UserRoles
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(x => x.Role.RoleClaims)
                .Where(x =>
                    !x.IsDeleted && (
                    //All
                    user.CompanyId == null || x.CompanyId == null ||
                    //Level 1 company
                    x.CompanyId == user.CompanyId ||
                    //Level 2 company
                    x.Company.CompanyId == user.CompanyId ||
                    //Level 3 company
                    x.Company.Company.CompanyId == user.CompanyId ||
                    //Level 4 company
                    x.Company.Company.Company.CompanyId == user.CompanyId
                //...
                )).Select(x => x.Role).ToListAsync();

            if (roles.IsNotEmpty())
            {
                claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));
                claims.AddRange(roles.SelectMany(x => x.RoleClaims.Select(y => new Claim(y.ClaimType, y.ClaimValue))));
            }
        }

        async Task AddCurrentCompanyId(User user, List<Claim> claims)
        {
            if (user.CurrentCompanyId != null)
            {
                var company = await _databaseContext.Companies
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.Id == user.CurrentCompanyId);

                if (company != null)
                {
                    claims.Add(new Claim(AppClaims.Meta.CurrentCompanyId, user.CurrentCompanyId.ToString()));
                    claims.Add(new Claim(AppClaims.Meta.CurrentCompanyName, company.Name));
                }
            }

            if (user.CompanyId.HasValue)
            {
                claims.Add(new Claim(AppClaims.Meta.BaseCompanyId, user.CompanyId.ToString()));

                var baseCompanyAccountType = (await _databaseContext.Companies
                    .IgnoreQueryFilters()
                    .Where(x => x.Id == user.CompanyId)
                    .Select(x => x.AccountType)
                    .FirstOrDefaultAsync());

                claims.Add(new Claim(AppClaims.Meta.BaseCompanyAccountType, baseCompanyAccountType.ToString()));
            }

            bool hasMultipleCompanies = _databaseContext.Companies
                .IgnoreQueryFilters()
                .Where(x => !x.IsDeleted)
                .Where(x => x.CompanyId == user.CompanyId)
                .Any();

            if (hasMultipleCompanies)
                claims.Add(new Claim(AppClaims.Meta.HasMultipleCompanies, hasMultipleCompanies.ToString()));
        }
    }
}
