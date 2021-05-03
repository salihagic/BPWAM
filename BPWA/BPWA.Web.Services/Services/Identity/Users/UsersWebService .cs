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
                       .Include(x => x.CompanyUsers)
                       .ThenInclude(x => x.Company)
                       .Include(x => x.BusinessUnitUsers)
                       .ThenInclude(x => x.BusinessUnit)
                       .Include(x => x.UserRoles)
                       .ThenInclude(x => x.Role)
                       .ThenInclude(x => x.Company)
                       .Include(x => x.UserRoles)
                       .ThenInclude(x => x.Role)
                       .ThenInclude(x => x.BusinessUnit);
        }

        public override IQueryable<User> BuildIncludes(IQueryable<User> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.City);
        }

        public override IQueryable<User> BuildQueryConditions(IQueryable<User> Query, UserSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(Query, searchModel)
                .WhereIf(CurrentUser.CurrentCompanyId().HasValue, x => x.CompanyUsers.Any(y => y.CompanyId == CurrentUser.CurrentCompanyId()) || x.BusinessUnitUsers.Any(y => y.BusinessUnit.CompanyId == CurrentUser.CurrentCompanyId()))
                .WhereIf(CurrentUser.CurrentBusinessUnitId().HasValue, x => x.BusinessUnitUsers.Any(y => y.BusinessUnit.Id == CurrentUser.CurrentBusinessUnitId()));
        }

        public override async Task<User> GetEntityById(string id, bool shouldTranslate = true, bool includeRelated = true)
        {
            var result = await base.GetEntityById(id, shouldTranslate, includeRelated);

            result.UserRoles ??= new List<UserRole>();

            result.UserRoles.ForEach(x =>
            {
                if (x.Role.Company != null)
                    x.Role.Name += $" ({x.Role.Company.Name})";
                if (x.Role.BusinessUnit != null)
                    x.Role.Name += $" ({x.Role.BusinessUnit.Name})";
            });

            return result;
        }

        public async Task<UserDTO> Add(UserAddModel model)
        {
            var entity = Mapper.Map<User>(model);
            var result = await base.Add(entity);

            await ManageRelatedEntities<UserRole, string, string>(result.Id, model.RoleIds, x => x.UserId, x => x.RoleId);
            await ManageRelatedEntities<CompanyUser, string, int>(result.Id, model.CompanyIds, x => x.UserId, x => x.CompanyId);
            await ManageRelatedEntities<BusinessUnitUser, string, int>(result.Id, model.BusinessUnitIds, x => x.UserId, x => x.BusinessUnitId);

            return result;
        }

        public async Task<UserDTO> Update(UserUpdateModel model)
        {
            var entity = await GetEntityById(model.Id, false, false);
            Mapper.Map(model, entity);
            var result = await base.Update(entity);

            await ManageRelatedEntities<UserRole, string, string>(result.Id, model.RoleIds, x => x.UserId, x => x.RoleId);

            if (!CurrentUser.CurrentCompanyId().HasValue)
            {
                await ManageRelatedEntities<CompanyUser, string, int>(result.Id, model.CompanyIds, x => x.UserId, x => x.CompanyId);
            }
            if (!CurrentUser.CurrentBusinessUnitId().HasValue)
            {
                await ManageRelatedEntities<BusinessUnitUser, string, int>(result.Id, model.BusinessUnitIds, x => x.UserId, x => x.BusinessUnitId);
            }

            return result;
        }

        public async Task ToggleCurrentCompany(ToggleCurrentCompanyModel model)
        {
            //TODO: Check if the user can change to selected company
            //if (!CurrentUser.HasGodMode() && !(CurrentUser.CompanyIds().Count > 1))
            //    return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            var currentUserResult = await GetEntityById(CurrentUser.Id(), false, false);

            try
            {
                currentUserResult.CurrentCompanyId = model.CompanyId;
                currentUserResult.CurrentBusinessUnitId = null;

                DatabaseContext.Users.Update(currentUserResult);
                await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Translations.There_was_an_error_while_trying_to_change_current_company);
            }

            await RefreshSignIn();
        }

        public async Task ToggleCurrentBusinessUnit(ToggleCurrentBusinessUnitModel model)
        {
            //TODO: Check if the user can change to selected business unit
            //if (!CurrentUser.HasGodMode() && !CurrentUser.HasCompanyGodMode() && !(CurrentUser.BusinessUnitIds().Count > 1))
            //    return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_business_unit);

            var currentUserResult = await GetEntityById(CurrentUser.Id(), false, false);

            try
            {
                currentUserResult.CurrentBusinessUnitId = model.BusinessUnitId;

                if (model.BusinessUnitId.HasValue)
                {
                    var companyId = DatabaseContext.BusinessUnits.FirstOrDefault(x => x.Id == model.BusinessUnitId)?.CompanyId;
                    currentUserResult.CurrentCompanyId = companyId;
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

                        currentUserResult.CurrentCompanyId = firstCompany.CompanyId;
                    }
                }

                DatabaseContext.Users.Update(currentUserResult);
                await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Translations.There_was_an_error_while_trying_to_change_current_business_unit);
            }

            await RefreshSignIn();

            return;
        }

        public async Task RefreshSignIn()
        {
            var currentUserResult = await GetEntityById(CurrentUser.Id());

            await SignInManager.RefreshSignInAsync(currentUserResult);
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
