using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class RolesWebService : RolesService, IRolesWebService
    {
        private ICurrentUser _currentUser;

        public RolesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            RoleManager<Role> roleManager,
            ICurrentUser currentUser
            ) : base(databaseContext, mapper, roleManager)
        {
            _currentUser = currentUser;
        }

        public override IQueryable<Role> BuildQueryConditions(IQueryable<Role> Query, RoleSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(Query, searchModel)
                       .WhereIf(_currentUser.CurrentCompanyId().HasValue, x => x.CompanyId == _currentUser.CurrentCompanyId() || x.BusinessUnit.CompanyId == _currentUser.CurrentCompanyId())
                       .WhereIf(_currentUser.CurrentBusinessUnitId().HasValue, x => x.BusinessUnitId == _currentUser.CurrentBusinessUnitId());
        }

        public override IQueryable<Role> BuildIncludesById(string id, IQueryable<Role> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.RoleClaims)
                       .Include(x => x.Company)
                       .Include(x => x.BusinessUnit);
        }

        public override IQueryable<Role> BuildIncludes(IQueryable<Role> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.Company)
                       .Include(x => x.BusinessUnit);
        }

        public override async Task<Result<Role>> AddEntity(Role entity)
        {
            entity.CompanyId = _currentUser.CurrentCompanyId();
            entity.BusinessUnitId = _currentUser.CurrentBusinessUnitId();

            return await base.AddEntity(entity);
        }

        public override async Task<Result<Role>> UpdateEntity(Role entity)
        {
            var currentRoleClaims = await DatabaseContext.RoleClaims.Where(x => x.RoleId == entity.Id).ToListAsync();

            if (currentRoleClaims.IsNotEmpty())
            {
                //Delete (Hard delete because of Identity generating user claims on login)
                var roleClaimsToDelete = currentRoleClaims.Where(x => !entity.RoleClaims?.Any(y => y.ClaimValue == x.ClaimValue) ?? true).ToList();
                DatabaseContext.RoleClaims.RemoveRange(roleClaimsToDelete);
                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.RoleClaims = entity.RoleClaims.Where(x => !currentRoleClaims.Any(y => y.ClaimValue == x.ClaimValue)).ToList();
            }

            return await base.UpdateEntity(entity);
        }

        public override Task<Result> Delete(Role entity)
        {
            var userRoles = DatabaseContext.UserRoles.Where(x => x.RoleId == entity.Id);
            var roleClaims = DatabaseContext.RoleClaims.Where(x => x.RoleId == entity.Id);

            DatabaseContext.RemoveRange(userRoles);
            DatabaseContext.RemoveRange(roleClaims);

            return base.Delete(entity);
        }
    }
}
