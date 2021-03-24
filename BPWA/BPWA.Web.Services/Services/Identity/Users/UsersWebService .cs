﻿using AutoMapper;
using BPWA.Common.Resources;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Identity;
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

        public async Task<Result> ToggleCurrentCompany(ToggleCurrentCompanyModel model)
        {
            var currentUser = await GetEntityById(CurrentUser.Id());

            if (currentUser == null)
                return Result.Failed(Translations.User_not_found);

            try
            {
                currentUser.CurrentCompanyId = model.CompanyId;
                if (!model.CompanyId.HasValue)
                    currentUser.CurrentBusinessUnitId = null;

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
            var currentUser = await GetEntityById(CurrentUser.Id());

            if (currentUser == null)
                return Result.Failed(Translations.User_not_found);

            try
            {
                currentUser.CurrentBusinessUnitId = model.BusinessUnitId;

                if (model.BusinessUnitId.HasValue)
                {
                    var companyId = DatabaseContext.BusinessUnits.FirstOrDefault(x => x.Id == model.BusinessUnitId)?.CompanyId;
                    currentUser.CurrentCompanyId = companyId;
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
    }
}
