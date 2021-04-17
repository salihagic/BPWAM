using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Extensions;
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
            CurrentUser currentUser,
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
                       .Include(x => x.City);
        }

        public override IQueryable<User> BuildIncludes(IQueryable<User> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.City);
        }

        //public async Task<Result<UserUpdateModel>> PrepareForUpdate(UserUpdateModel model = null)
        //{
        //    model ??= new UserUpdateModel();

        //    if (model.RoleIds.IsNotEmpty())
        //    {
        //        try
        //        {
        //            model.RoleIdsSelectList = await DatabaseContext.Roles
        //                .Where(x => model.RoleIds.Contains(x.Id))
        //                .Select(x => new SelectListItem
        //                {
        //                    Value = x.Id.ToString(),
        //                    Text = x.CompanyId.HasValue ? $"{x.Name} ({x.Company.Name})" : x.BusinessUnitId.HasValue ? $"{x.Name} ({x.BusinessUnit.Name})" : x.Name,
        //                }).ToListAsync();
        //        }
        //        catch (Exception e)
        //        {
        //            return Result.Failed<UserUpdateModel>("Could not load roles");
        //        }
        //    }
        //    if (model.CompanyIds.IsNotEmpty())
        //    {
        //        try
        //        {
        //            model.CompanyIdsSelectList = await DatabaseContext.Companies
        //            .Where(x => model.CompanyIds.Contains(x.Id))
        //            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
        //        }
        //        catch (Exception e)
        //        {
        //            return Result.Failed<UserUpdateModel>("Could not load companies");
        //        }
        //    }
        //    if (model.BusinessUnitIds.IsNotEmpty())
        //    {
        //        try
        //        {
        //            model.BusinessUnitIdsSelectList = await DatabaseContext.BusinessUnits
        //            .Where(x => model.BusinessUnitIds.Contains(x.Id))
        //            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
        //        }
        //        catch (Exception e)
        //        {
        //            return Result.Failed<UserUpdateModel>("Could not load business units");
        //        }
        //    }

        //    model.RoleIdsSelectList ??= new List<SelectListItem>();
        //    model.CompanyIdsSelectList ??= new List<SelectListItem>();
        //    model.BusinessUnitIdsSelectList ??= new List<SelectListItem>();

        //    return Result.Success(model);
        //}

        //public override async Task<Result<User>> UpdateEntity(User entity)
        //{
        //    if (!CurrentUser.CurrentCompanyId().HasValue)
        //    {
        //        var currentCompanyUsers = await DatabaseContext.CompanyUsers.Where(x => x.UserId == entity.Id && !x.IsDeleted).ToListAsync();

        //        if (currentCompanyUsers.IsNotEmpty())
        //        {
        //            //Delete
        //            var currentCompanyUsersToDelete = currentCompanyUsers.Where(x => !entity.CompanyUsers?.Any(y => y.CompanyId == x.CompanyId) ?? true).ToList();
        //            currentCompanyUsersToDelete?.ForEach(x => x.IsDeleted = true);

        //            await DatabaseContext.SaveChangesAsync();

        //            //Only leave the new ones
        //            entity.CompanyUsers = entity.CompanyUsers?.Where(x => !currentCompanyUsers.Any(y => y.CompanyId == x.CompanyId))?.ToList();
        //        }
        //    }

        //    if (!CurrentUser.CurrentBusinessUnitId().HasValue)
        //    {
        //        var currentBusinessUnitUsers = await DatabaseContext.BusinessUnitUsers
        //            .Where(x => x.UserId == entity.Id && !x.IsDeleted && x.BusinessUnit.CompanyId == CurrentUser.CurrentCompanyId())
        //            .ToListAsync();

        //        if (currentBusinessUnitUsers.IsNotEmpty())
        //        {
        //            //Delete
        //            var currentBusinessUnitUsersToDelete = currentBusinessUnitUsers.Where(x => !entity.BusinessUnitUsers?.Any(y => y.BusinessUnitId == x.BusinessUnitId) ?? true).ToList();
        //            currentBusinessUnitUsersToDelete?.ForEach(x => x.IsDeleted = true);

        //            await DatabaseContext.SaveChangesAsync();

        //            //Only leave the new ones
        //            entity.BusinessUnitUsers = entity?.BusinessUnitUsers.Where(x => !currentBusinessUnitUsers.Any(y => y.BusinessUnitId == x.BusinessUnitId))?.ToList();
        //        }
        //    }

        //    if (entity.UserRoles.IsNotEmpty())
        //    {
        //        var selectedRoleIds = entity.UserRoles.Select(y => y.RoleId);
        //        var roles = await DatabaseContext.Roles.Where(x => selectedRoleIds.Contains(x.Id) && !x.IsDeleted).ToListAsync();
        //        var businessUnitRoles = roles.Where(x => !x.CompanyId.HasValue && x.BusinessUnitId.HasValue).ToList();

        //        #region System roles

        //        if (CurrentUser.HasGodMode() || CurrentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.UsersManagement))
        //        {
        //            var selectedSystemRoles = roles.Where(x => !x.CompanyId.HasValue && !x.BusinessUnitId.HasValue).ToList();
        //            var currentUserSystemRoles = await DatabaseContext.UserRoles
        //                .Where(x => x.UserId == entity.Id && !x.IsDeleted)
        //                .ToListAsync();

        //            if (currentUserSystemRoles.IsNotEmpty())
        //            {
        //                //Delete
        //                var currentUserSystemRolesToDelete = currentUserSystemRoles.Where(x => !selectedSystemRoles?.Any(y => y.Id == x.RoleId) ?? true).ToList();
        //                DatabaseContext.UserRoles.RemoveRange(currentUserSystemRolesToDelete);
        //                await DatabaseContext.SaveChangesAsync();
        //            }

        //            //Leave the new ones
        //            entity.UserRoles = entity.UserRoles.Where(x => !currentUserSystemRoles.Any(y => y.RoleId == x.RoleId)).ToList();
        //        }

        //        #endregion

        //        #region Company roles

        //        if (CurrentUser.CurrentCompanyId().HasValue || CurrentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyUsersManagement))
        //        {
        //            var selectedCompanyRoles = roles.Where(x => x.CompanyId.HasValue && !x.BusinessUnitId.HasValue).ToList();

        //            var currentCompanyUser = await DatabaseContext.CompanyUsers
        //                .Where(x => x.UserId == entity.Id && x.CompanyId == CurrentUser.CurrentCompanyId() && !x.IsDeleted)
        //                .FirstOrDefaultAsync();

        //            if (currentCompanyUser == null)
        //            {
        //                currentCompanyUser = new CompanyUser { CompanyId = CurrentUser.CurrentCompanyId().GetValueOrDefault() };

        //                entity.CompanyUsers ??= new List<CompanyUser>();
        //                entity.CompanyUsers.Add(currentCompanyUser);
        //            }

        //            //Delete
        //            var currentCompanyUserRolesToDelete = currentCompanyUser.CompanyUserRoles.Where(x => !selectedCompanyRoles?.Any(y => y.Id == x.RoleId) ?? true).ToList();
        //            DatabaseContext.CompanyUserRoles.RemoveRange(currentCompanyUserRolesToDelete);
        //            await DatabaseContext.SaveChangesAsync();

        //            //Leave the new ones
        //            currentCompanyUser.CompanyUserRoles = selectedCompanyRoles
        //                .Where(x => !currentCompanyUser.CompanyUserRoles.Any(y => y.RoleId == x.Id))
        //                .Select(x => new CompanyUserRole { CompanyUserId = CurrentUser.CurrentCompanyId().GetValueOrDefault() })
        //                .ToList();
        //            await DatabaseContext.SaveChangesAsync();
        //        }

        //        #endregion

        //        #region BusinessUnit roles

        //        if (CurrentUser.CurrentBusinessUnitId().HasValue || CurrentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitUsersManagement))
        //        {
        //            var selectedBusinessUnitRoles = roles.Where(x => !x.CompanyId.HasValue && x.BusinessUnitId.HasValue).ToList();

        //            var currentBusinessUnitUser = await DatabaseContext.BusinessUnitUsers
        //                .Where(x => x.UserId == entity.Id && x.BusinessUnitId == CurrentUser.CurrentBusinessUnitId() && !x.IsDeleted)
        //                .FirstOrDefaultAsync();

        //            if (currentBusinessUnitUser == null)
        //            {
        //                currentBusinessUnitUser = new BusinessUnitUser { BusinessUnitId = CurrentUser.CurrentBusinessUnitId().GetValueOrDefault() };

        //                entity.BusinessUnitUsers ??= new List<BusinessUnitUser>();
        //                entity.BusinessUnitUsers.Add(currentBusinessUnitUser);
        //            }

        //            //Delete
        //            var currentBusinessUnitUserRolesToDelete = currentBusinessUnitUser.BusinessUnitUserRoles.Where(x => !selectedBusinessUnitRoles?.Any(y => y.Id == x.RoleId) ?? true).ToList();
        //            DatabaseContext.BusinessUnitUserRoles.RemoveRange(currentBusinessUnitUserRolesToDelete);
        //            await DatabaseContext.SaveChangesAsync();

        //            //Leave the new ones
        //            currentBusinessUnitUser.BusinessUnitUserRoles = selectedBusinessUnitRoles
        //                .Where(x => !currentBusinessUnitUser.BusinessUnitUserRoles.Any(y => y.RoleId == x.Id))
        //                .Select(x => new BusinessUnitUserRole { BusinessUnitUserId = CurrentUser.CurrentBusinessUnitId().GetValueOrDefault() })
        //                .ToList();
        //            await DatabaseContext.SaveChangesAsync();
        //        }

        //        #endregion
        //    }

        //    return await base.UpdateEntity(entity);
        //}

        public async Task<Result> ToggleCurrentCompany(ToggleCurrentCompanyModel model)
        {
            //TODO: Check if the user can change to selected company
            //if (!CurrentUser.HasGodMode() && !(CurrentUser.CompanyIds().Count > 1))
            //    return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            var currentUserResult = await GetEntityById(CurrentUser.Id());

            if (!currentUserResult.IsSuccess)
                return Result.Failed(currentUserResult.GetErrorMessages());

            try
            {
                currentUserResult.Item.CurrentCompanyId = model.CompanyId;
                currentUserResult.Item.CurrentBusinessUnitId = null;

                DatabaseContext.Users.Update(currentUserResult.Item);
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
            //TODO: Check if the user can change to selected business unit
            //if (!CurrentUser.HasGodMode() && !CurrentUser.HasCompanyGodMode() && !(CurrentUser.BusinessUnitIds().Count > 1))
            //    return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_business_unit);

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
                else
                {
                    //Default to first company
                    var isCompanyUserForCurrentCompany = await DatabaseContext.CompanyUsers
                        .AnyAsync(x => x.UserId == CurrentUser.Id() && x.CompanyId == CurrentUser.CurrentCompanyId());

                    if (!isCompanyUserForCurrentCompany && !CurrentUser.HasGodMode())
                    {
                        var firstCompany = await DatabaseContext.CompanyUsers
                        .FirstOrDefaultAsync(x => x.UserId == CurrentUser.Id());

                        if (firstCompany == null)
                            return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_business_unit);

                        currentUserResult.Item.CurrentCompanyId = firstCompany.CompanyId;
                    }
                }

                DatabaseContext.Users.Update(currentUserResult.Item);
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

        public async Task<Result<ResetPasswordModel>> PrepareForResetPassword(string userId, string token)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failed<ResetPasswordModel>("Failed to load user");

            var model = new ResetPasswordModel
            {
                UserId = userId,
                Token = token
            };

            return Result.Success(model);
        }

        public async Task<Result> ResetPassword(ResetPasswordModel model)
        {
            var user = await UserManager.FindByIdAsync(model.UserId);

            if (user == null)
                return Result.Failed("Failed to load user");

            var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
                return Result.Failed(result.Errors.Select(x => x.Description).ToList());

            return Result.Success();
        }

        public async Task SignOut()
        {
            await SignInManager.SignOutAsync();
        }
    }
}
