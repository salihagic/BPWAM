using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Services;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class UsersWebService : UsersService, IUsersWebService
    {
        public UsersWebService(
            IMapper mapper,
            DatabaseContext databaseContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICurrentUser currentUser,
            IPasswordGeneratorService passwordGeneratorService,
            IEmailService emailService,
            RouteSettings routeSettings
            ) : base(
                databaseContext,
                mapper,
                userManager,
                signInManager,
                currentUser,
                passwordGeneratorService,
                emailService,
                routeSettings
                )
        {
        }

        public override IQueryable<User> BuildIncludesById(string id, IQueryable<User> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.City)
                       .Include(x => x.UserRoles)
                       .ThenInclude(x => x.Role)
                       .ThenInclude(x => x.Company);
        }

        public override IQueryable<User> BuildIncludes(IQueryable<User> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.City);
        }

        public async Task<User> GetEntityByIdWithoutQueryFilters(string id)
        {
            return await DatabaseContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<User> GetEntityById(string id, bool shouldTranslate = true, bool includeRelated = true)
        {
            var result = await base.GetEntityById(id, shouldTranslate, includeRelated);

            result.UserRoles ??= new List<UserRole>();

            result.UserRoles.ForEach(x =>
            {
                if (x.Role.Company != null)
                    x.Role.Name += $" ({x.Role.Company.Name})";
            });

            return result;
        }

        public async Task<UserDTO> Add(UserAddModel model)
        {
            var entity = Mapper.Map<User>(model);
            var result = await base.Add(entity);

            await ManageRelatedEntities<UserRole, string, string>(result.Id, model.RoleIds, x => x.UserId, x => x.RoleId);

            return result;
        }

        public async Task<UserDTO> Update(UserUpdateModel model)
        {
            var entity = await GetEntityById(model.Id, false, false);
            Mapper.Map(model, entity);
            var result = await base.Update(entity);

            await ManageRelatedEntities<UserRole, string, string>(result.Id, model.RoleIds, x => x.UserId, x => x.RoleId);

            return result;
        }
    }
}
