using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Resources;
using BPWA.Common.Services;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class AccountsWebService :
        AccountsService,
        IAccountsWebService
    {
        public AccountsWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICurrentUser currentUser,
            IEmailService emailService,
            RouteSettings routeSettings,
            IUsersService usersService
            ) : base(
                databaseContext,
                mapper,
                userManager,
                signInManager,
                currentUser,
                emailService,
                routeSettings,
                usersService
                )
        {
        }

        #region Update account 

        public async Task<AccountUpdateModel> PrepareForUpdate()
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

        public async Task Update(AccountUpdateModel model)
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

        public async Task<User> GetUserByIdWithoutQueryFilters(string id)
        {
            return await DatabaseContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task ToggleCurrentCompany(ToggleCurrentCompanyModel model)
        {
            //TODO: Check if the user can change to selected company
            //if (!CurrentUser.HasGodMode() && !(CurrentUser.CompanyIds().Count > 1))
            //    return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            var currentUser = await GetUserByIdWithoutQueryFilters(CurrentUser.Id());

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
            var currentUser = await GetUserByIdWithoutQueryFilters(CurrentUser.Id());

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
