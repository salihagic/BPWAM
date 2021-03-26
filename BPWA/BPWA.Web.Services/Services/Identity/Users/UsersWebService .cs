using AutoMapper;
using BPWA.Common.Resources;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public override IQueryable<User> BuildIncludesById(string id, IQueryable<User> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.City);
        }

        public override IQueryable<User> BuildIncludes(IQueryable<User> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.City);
        }

        public async Task<Result> ToggleCurrentCompany(ToggleCurrentCompanyModel model)
        {
            if(!CurrentUser.HasGodMode() && !(CurrentUser.CompanyIds().Count > 1))
                return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            var currentUserResult = await GetEntityById(CurrentUser.Id());

            if (!currentUserResult.IsSuccess)
                return Result.Failed(currentUserResult.GetErrorMessages());

            try
            {
                currentUserResult.Item.CurrentCompanyId = model.CompanyId;
                if (!model.CompanyId.HasValue)
                    currentUserResult.Item.CurrentBusinessUnitId = null;

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

        public async Task<Result> ToggleCurrentBusinessUnit(ToggleCurrentBusinessUnitModel model)
        {
            if (!CurrentUser.HasGodMode() && !(CurrentUser.BusinessUnitIds().Count > 1))
                return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            var currentUserResult = await GetEntityById(CurrentUser.Id());

            if (!currentUserResult.IsSuccess)
                return Result.Failed(currentUserResult.GetErrorMessages());

            try
            {
                currentUserResult.Item.CurrentBusinessUnitId = model.BusinessUnitId;

                if (model.BusinessUnitId.HasValue)
                {
                    var companyId = DatabaseContext.BusinessUnits.FirstOrDefault(x => x.Id == model.BusinessUnitId)?.CompanyId;
                    currentUserResult.Item.CurrentCompanyId = companyId;
                }

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

        public async Task<Result> RefreshSignIn()
        {
            var currentUserResult = await GetEntityById(CurrentUser.Id());

            if (!currentUserResult.IsSuccess)
                return Result.Failed(currentUserResult.GetErrorMessages());

            await SignInManager.RefreshSignInAsync(currentUserResult.Item);

            return Result.Success();
        }

        public async Task SignOut()
        {
            await SignInManager.SignOutAsync();
        }
    }
}
