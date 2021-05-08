using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Resources;
using BPWA.Common.Services;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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

        #region Update account 

        public async Task<AccountUpdateModel> PrepareForUpdateAccount()
        {
            try
            {
                var user = await DatabaseContext.Users
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Include(x => x.City)
                    .FirstOrDefaultAsync(x => x.Id == CurrentUser.Id());

                var model = Mapper.Map<AccountUpdateModel>(user);

                return model;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load the user");
            }
        }

        public async Task UpdateAccount(AccountUpdateModel model)
        {
            try
            {
                var user = await DatabaseContext.Users
                    .IgnoreQueryFilters()
                    .Include(x => x.City)
                    .FirstOrDefaultAsync(x => x.Id == CurrentUser.Id());

                Mapper.Map(model, user);

                await DatabaseContext.SaveChangesAsync();

                await RefreshSignIn();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to update account");
            }
        }

        #endregion

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

        public async Task ToggleCurrentCompany(ToggleCurrentCompanyModel model)
        {
            //TODO: Check if the user can change to selected company
            //if (!CurrentUser.HasGodMode() && !(CurrentUser.CompanyIds().Count > 1))
            //    return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            var currentUser = await GetEntityByIdWithoutQueryFilters(CurrentUser.Id());

            if (model.CompanyId == 0)
                model.CompanyId = currentUser.CompanyId;

            try
            {
                currentUser.CurrentCompanyId = model.CompanyId;

                DatabaseContext.Users.Update(currentUser);
                await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Translations.There_was_an_error_while_trying_to_change_current_company);
            }

            await RefreshSignIn();
        }

        public async Task RefreshSignIn()
        {
            var currentUser = await GetEntityByIdWithoutQueryFilters(CurrentUser.Id());

            await SignInManager.RefreshSignInAsync(currentUser);
        }

        public async Task<ResetPasswordModel> PrepareForResetPassword(string userId, string token)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("Failed to load user");

            var model = new ResetPasswordModel
            {
                UserId = userId,
                Token = token
            };

            return model;
        }

        public async Task ResetPassword(ResetPasswordModel model)
        {
            var user = await UserManager.FindByIdAsync(model.UserId);

            if (user == null)
                throw new Exception("Failed to load user");

            var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
        }

        public async Task SignOut()
        {
            await SignInManager.SignOutAsync();
        }
    }
}
