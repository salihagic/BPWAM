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
            var claims = new List<Claim>();

            await AddBasicInfo(user, claims);
            await AddCurrentCompanyId(user, claims);
            await AddCurrentBusinessUnitId(user, claims);
            await AddCompanyIds(user, claims);
            await AddBusinessUnitIds(user, claims);

            (principal.Identity as ClaimsIdentity).AddClaims(claims);

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
        
        async Task AddCurrentCompanyId(User user, List<Claim> claims)
        {
            if (user.CurrentCompanyId != null)
            {
                var company = await _databaseContext.Companies
                                                    .FirstOrDefaultAsync(x => x.Id == user.CurrentCompanyId);

                claims.Add(new Claim(AppClaims.Meta.CurrentCompanyId, user.CurrentCompanyId.ToString()));
                claims.Add(new Claim(AppClaims.Meta.CurrentCompanyName, company.Name));

                var companyRoles = _databaseContext.CompanyUserRoles
                    .Where(x => x.CompanyUser.UserId == user.Id && x.CompanyUser.CompanyId == user.CurrentCompanyId)
                    .Select(x => x.Role.Name);
                if (companyRoles.IsNotEmpty())
                    claims.AddRange(companyRoles.Select(x => new Claim(ClaimTypes.Role, x)));

                var companyRoleClaims = _databaseContext.CompanyUserRoles
                    .Where(x => x.CompanyUser.UserId == user.Id && x.CompanyUser.CompanyId == user.CurrentCompanyId)
                    .SelectMany(x => x.Role.RoleClaims);
                if (companyRoleClaims.IsNotEmpty())
                    claims.AddRange(companyRoleClaims.Select(x => new Claim(x.ClaimType, x.ClaimValue)));
            }
        }

        async Task AddCurrentBusinessUnitId(User user, List<Claim> claims)
        {
            if (user.CurrentBusinessUnitId != null)
            {
                var businessUnit = await _databaseContext.BusinessUnits.FirstOrDefaultAsync(x => x.Id == user.CurrentBusinessUnitId);

                claims.Add(new Claim(AppClaims.Meta.CurrentBusinessUnitId, user.CurrentBusinessUnitId.ToString()));
                claims.Add(new Claim(AppClaims.Meta.CurrentBusinessUnitName, businessUnit.Name));

                var businessUnitRoles = _databaseContext.BusinessUnitUserRoles
                    .Where(x => x.BusinessUnitUser.UserId == user.Id && x.BusinessUnitUser.BusinessUnitId == user.CurrentBusinessUnitId)
                    .Select(x => x.Role.Name);
                if (businessUnitRoles.IsNotEmpty())
                    claims.AddRange(businessUnitRoles.Select(x => new Claim(ClaimTypes.Role, x)));

                var businessUnitRoleClaims = _databaseContext.BusinessUnitUserRoles
                    .Where(x => x.BusinessUnitUser.UserId == user.Id && x.BusinessUnitUser.BusinessUnitId == user.CurrentBusinessUnitId)
                    .SelectMany(x => x.Role.RoleClaims);
                if (businessUnitRoleClaims.IsNotEmpty())
                    claims.AddRange(businessUnitRoleClaims.Select(x => new Claim(x.ClaimType, x.ClaimValue)));
            }
        }

        async Task AddCompanyIds(User user, List<Claim> claims)
        {
            var companyIds = await _databaseContext.CompanyUsers
                                       .Where(x => x.UserId == user.Id)
                                       .Select(x => x.CompanyId)
                                       .ToListAsync();

            if (companyIds.IsNotEmpty())
                claims.AddRange(companyIds.Select(x => new Claim(AppClaims.Meta.CompanyIds, x.ToString())));
        }

        async Task AddBusinessUnitIds(User user, List<Claim> claims)
        {
            var businessUnitIds = await _databaseContext.BusinessUnitUsers
                                        .Where(x => x.UserId == user.Id)
                                        .Select(x => x.BusinessUnitId)
                                        .ToListAsync();
            if (businessUnitIds.IsNotEmpty())
                claims.AddRange(businessUnitIds.Select(x => new Claim(AppClaims.Meta.BusinessUnitIds, x.ToString())));
        }
    }
}
