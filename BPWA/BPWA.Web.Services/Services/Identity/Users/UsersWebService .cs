using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Common.Resources;
using BPWA.Common.Services;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            CurrentUser loggedUserService,
            IPasswordGeneratorService passwordGeneratorService,
            IEmailService emailService
            ) : base(
                databaseContext,
                mapper,
                userManager,
                signInManager,
                loggedUserService,
                passwordGeneratorService,
                emailService
                )
        {
        }

        public override IQueryable<User> BuildIncludesById(string id, IQueryable<User> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.City)
                       .Include(x => x.CompanyUsers)
                       .ThenInclude(x => x.Company)
                       .Include(x => x.BusinessUnitUsers)
                       .ThenInclude(x => x.BusinessUnit)
                       .Include(x => x.UserRoles)
                       .ThenInclude(x => x.Role);
        }

        public override IQueryable<User> BuildIncludes(IQueryable<User> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.City);
        }

        public async Task<Result<UserAddModel>> PrepareForAdd(UserAddModel model = null)
        {
            model ??= new UserAddModel();

            if (model.RoleIds.IsNotEmpty())
            {
                try
                {
                    model.RoleIdsSelectList = await DatabaseContext.Roles
                    .Where(x => model.RoleIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<UserAddModel>("Could not load roles");
                }
            }
            if (model.CompanyIds.IsNotEmpty())
            {
                try
                {
                    model.CompanyIdsSelectList = await DatabaseContext.Companies
                    .Where(x => model.CompanyIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<UserAddModel>("Could not load companies");
                }
            }
            if (model.BusinessUnitIds.IsNotEmpty())
            {
                try
                {
                    model.BusinessUnitIdsSelectList = await DatabaseContext.BusinessUnits
                    .Where(x => model.BusinessUnitIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<UserAddModel>("Could not load business units");
                }
            }

            model.RoleIdsSelectList ??= new List<SelectListItem>();
            model.CompanyIdsSelectList ??= new List<SelectListItem>();
            model.BusinessUnitIdsSelectList ??= new List<SelectListItem>();

            return Result.Success(model);
        }

        public async Task<Result<UserUpdateModel>> PrepareForUpdate(UserUpdateModel model = null)
        {
            model ??= new UserUpdateModel();

            if (model.RoleIds.IsNotEmpty())
            {
                try
                {
                    model.RoleIdsSelectList = await DatabaseContext.Roles
                    .Where(x => model.RoleIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<UserUpdateModel>("Could not load roles");
                }
            }
            if (model.CompanyIds.IsNotEmpty())
            {
                try
                {
                    model.CompanyIdsSelectList = await DatabaseContext.Companies
                    .Where(x => model.CompanyIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<UserUpdateModel>("Could not load companies");
                }
            }
            if (model.BusinessUnitIds.IsNotEmpty())
            {
                try
                {
                    model.BusinessUnitIdsSelectList = await DatabaseContext.BusinessUnits
                    .Where(x => model.BusinessUnitIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<UserUpdateModel>("Could not load business units");
                }
            }

            model.RoleIdsSelectList ??= new List<SelectListItem>();
            model.CompanyIdsSelectList ??= new List<SelectListItem>();
            model.BusinessUnitIdsSelectList ??= new List<SelectListItem>();

            return Result.Success(model);
        }

        public override async Task<Result<UserDTO>> Update(User entity)
        {
            var currentUserRoles = await DatabaseContext.UserRoles.Where(x => x.UserId == entity.Id && !x.IsDeleted).ToListAsync();

            if (currentUserRoles.IsNotEmpty())
            {
                //Delete
                var currentUserRolesToDelete = currentUserRoles.Where(x => !entity.UserRoles?.Any(y => y.RoleId == x.RoleId) ?? true).ToList();
                DatabaseContext.UserRoles.RemoveRange(currentUserRolesToDelete);

                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.UserRoles = entity.UserRoles.Where(x => !currentUserRoles.Any(y => y.RoleId == x.RoleId)).ToList();
            }

            var currentCompanyUsers = await DatabaseContext.CompanyUsers.Where(x => x.UserId == entity.Id && !x.IsDeleted).ToListAsync();

            if (currentCompanyUsers.IsNotEmpty())
            {
                //Delete
                var currentCompanyUsersToDelete = currentCompanyUsers.Where(x => !entity.CompanyUsers?.Any(y => y.CompanyId == x.CompanyId) ?? true).ToList();
                currentCompanyUsersToDelete?.ForEach(x => x.IsDeleted = true);

                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.CompanyUsers = entity.CompanyUsers.Where(x => !currentCompanyUsers.Any(y => y.CompanyId == x.CompanyId)).ToList();
            }

            var currentBusinessUnitUsers = await DatabaseContext.BusinessUnitUsers.Where(x => x.UserId == entity.Id && !x.IsDeleted).ToListAsync();

            if (currentBusinessUnitUsers.IsNotEmpty())
            {
                //Delete
                var currentBusinessUnitUsersToDelete = currentBusinessUnitUsers.Where(x => !entity.BusinessUnitUsers?.Any(y => y.BusinessUnitId == x.BusinessUnitId) ?? true).ToList();
                currentBusinessUnitUsersToDelete?.ForEach(x => x.IsDeleted = true);

                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.BusinessUnitUsers = entity.BusinessUnitUsers.Where(x => !currentBusinessUnitUsers.Any(y => y.BusinessUnitId == x.BusinessUnitId)).ToList();
            }

            return await base.Update(entity);
        }

        public async Task<Result> ToggleCurrentCompany(ToggleCurrentCompanyModel model)
        {
            if (!CurrentUser.HasGodMode() && !(CurrentUser.CompanyIds().Count > 1))
                return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            var currentUserResult = await GetEntityById(CurrentUser.Id());

            if (!currentUserResult.IsSuccess)
                return Result.Failed(currentUserResult.GetErrorMessages());

            try
            {
                currentUserResult.Item.CurrentCompanyId = model.CompanyId;
                currentUserResult.Item.CurrentBusinessUnitId = null;

                await Update(currentUserResult.Item);
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

                await Update(currentUserResult.Item);
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
