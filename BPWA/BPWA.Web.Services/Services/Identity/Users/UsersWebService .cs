using AutoMapper;
using BPWA.Common.Resources;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.Web.Services.Services
{
    public class UsersWebService : UsersService, IUsersWebService
    {
        public UsersWebService(
            IMapper mapper,
            DatabaseContext databaseContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            CurrentUser loggedUserService
            ) : base(
                databaseContext,
                mapper,
                userManager,
                signInManager,
                loggedUserService
                )
        {
        }

        public async Task SignOut()
        {
            await SignInManager.SignOutAsync();
        }

        public async Task<Result> RefreshSignIn()
        {
            var currentUser = await GetEntityById(CurrentUser.Id());

            if (currentUser == null)
                return Result.Failed(Translations.User_not_found);

            await SignInManager.RefreshSignInAsync(currentUser);

            return Result.Success();
        }

        public async Task<Result> ToggleCompany(ToggleCompanyModel model)
        {
            var currentUser = await GetEntityById(CurrentUser.Id());

            if (currentUser == null)
                return Result.Failed(Translations.User_not_found);

            try
            {
                currentUser.CompanyId = model.CompanyId;
                await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);
            }

            var refreshSignInResult = await RefreshSignIn();

            if (!refreshSignInResult.IsSuccess)
                return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            return Result.Success();
        }

        public async Task<Result> ToggleBusinessUnit(ToggleBusinessUnitModel model)
        {
            var currentUser = await GetEntityById(CurrentUser.Id());

            if (currentUser == null)
                return Result.Failed(Translations.User_not_found);

            try
            {
                currentUser.BusinessUnitId = model.BusinessUnitId;
                await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_business_unit);
            }

            var refreshSignInResult = await RefreshSignIn();

            if (!refreshSignInResult.IsSuccess)
                return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_business_unit);

            return Result.Success();
        }
    }
}
