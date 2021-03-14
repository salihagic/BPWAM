using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.DAL.Services
{
    public class RolesWebService : RolesService, IRolesWebService
    {
        public RolesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            RoleManager<Role> roleManager
            ) : base(databaseContext, mapper, roleManager)
        {
        }

        public override IQueryable<Role> BuildIncludesById(string id, IQueryable<Role> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.RoleClaims.Where(y => !y.IsDeleted));
        }

        public override async Task<Result<RoleDTO>> Update(Role entity)
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

            return await base.Update(entity);
        }

        public override Task<Result> Delete(Role entity, bool softDelete = true)
        {
            var userRoles = DatabaseContext.UserRoles.Where(x => x.RoleId == entity.Id);
            var roleClaims = DatabaseContext.RoleClaims.Where(x => x.RoleId == entity.Id);

            DatabaseContext.RemoveRange(userRoles);
            DatabaseContext.RemoveRange(roleClaims);

            return base.Delete(entity, false);
        }
    }
}
