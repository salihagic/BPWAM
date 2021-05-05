﻿using BPWA.Common.Extensions;
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

            foreach (var claim in claimsIdentity.Claims.ToList())
                if (claim.Type == ClaimTypes.Role || claim.Type == AppClaimsHelper.Authorization.Type)
                    claimsIdentity.RemoveClaim(claim);

            await AddBasicInfo(user, claims);
            await AddRoles(user, claims);
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

        async Task AddRoles(User user, List<Claim> claims)
        {
            var roles = await _databaseContext.UserRoles
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(x => x.Role.RoleClaims)
                .Where(x => x.UserId == user.Id)
                .Where(x => x.Role.CompanyId == null || x.Role.CompanyId == user.CurrentCompanyId)
                .Select(x => x.Role)
                .ToListAsync();

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

            bool hasMultipleCompanies = _databaseContext.Companies
                .IgnoreQueryFilters()
                .Any(x => x.CompanyId == user.CompanyId);

            claims.Add(new Claim(AppClaims.Meta.HasMultipleCompanies, hasMultipleCompanies.ToString()));
        }
    }
}
