using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Common.Resources;
using BPWA.Common.Services;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class UsersService : IUsersService
    {
        protected readonly DatabaseContext DatabaseContext;
        protected readonly IMapper Mapper;
        protected IQueryable<User> Query { get; set; }
        protected readonly UserManager<User> UserManager;
        protected readonly SignInManager<User> SignInManager;
        protected readonly CurrentUser CurrentUser;
        protected readonly IPasswordGeneratorService PasswordGeneratorService;
        protected readonly IEmailService EmailService;

        public UsersService(
            DatabaseContext databaseContext,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            CurrentUser loggedUserService,
            IPasswordGeneratorService passwordGeneratorService,
            IEmailService emailService
            )
        {
            DatabaseContext = databaseContext;
            Mapper = mapper;
            Query = databaseContext.Users.AsQueryable();
            UserManager = userManager;
            SignInManager = signInManager;
            CurrentUser = loggedUserService;
            PasswordGeneratorService = passwordGeneratorService;
            EmailService = emailService;
        }

        public async Task<Result<User>> AddEntity(User entity, string password)
        {
            var result = await UserManager.CreateAsync(entity, password);

            if (!result.Succeeded)
                return Result.Failed<User>(result.Errors.Select(x => x.Description).ToList());

            return Result.Success(entity);
        }

        public async Task<Result<UserDTO>> AddToRole(User entity, string roleName)
        {
            var result = await UserManager.AddToRoleAsync(entity, roleName);

            if (!result.Succeeded)
                return Result.Failed<UserDTO>(result.Errors.Select(x => x.Description).ToList());

            var userDTO = Mapper.Map<UserDTO>(entity);

            return Result.Success(userDTO);
        }

        public async Task<Result<User>> GetEntityByUserNameOrEmail(string userName)
        {
            try
            {
                var user = (await UserManager.FindByNameAsync(userName)) ?? (await UserManager.FindByEmailAsync(userName));

                if (user == null)
                    return Result.Failed<User>(Translations.User_name_or_email_invalid);

                return Result.Success(user);
            }
            catch (Exception e)
            {
                return Result.Failed<User>(Translations.User_name_or_email_invalid);
            }
        }

        public async Task<Result<UserDTO>> SignIn(string userName, string password)
        {
            var userResult = await GetEntityByUserNameOrEmail(userName);

            if (!userResult.IsSuccess)
                return Result.Failed<UserDTO>(userResult.GetErrorMessages());

            var result = await SignInManager.PasswordSignInAsync(userResult.Item, password, true, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return Result.Failed<UserDTO>(Translations.Account_locked_out);
                else if (result.IsNotAllowed)
                    return Result.Failed<UserDTO>(Translations.Login_not_allowed);
                else if (result.RequiresTwoFactor)
                    return Result.Failed<UserDTO>(Translations.Login_required_two_factor);
                else
                    return Result.Failed<UserDTO>(Translations.User_name_or_email_invalid);
            }

            var userDTO = Mapper.Map<UserDTO>(userResult.Item);

            return Result.Success(userDTO);
        }

        public async Task<Result> UpdateTimezoneForCurrentUser(int timezoneUtcOffsetInMinutes)
        {
            var userResult = await GetEntityById(CurrentUser.Id());

            if (!userResult.IsSuccess)
                return Result.Failed(Translations.User_not_found);

            var timezoneInfo = TimeZoneInfo.GetSystemTimeZones()
                                               .Where(x => x.BaseUtcOffset == (new TimeSpan(0, timezoneUtcOffsetInMinutes, 0)))
                                               .FirstOrDefault();

            if (timezoneInfo != null)
            {
                userResult.Item.TimezoneId = timezoneInfo.Id;
                var result = await Update(userResult.Item);

                if (!result.IsSuccess)
                    return Result.Failed(result.GetErrorMessages());

                return Result.Success();
            }

            return Result.Failed(Translations.Failed_to_update_timezone);
        }

        #region Base

        virtual public IQueryable<User> BuildQueryConditions(IQueryable<User> Query, UserSearchModel searchModel = null)
        {
            if (searchModel == null)
                return Query;

            return Query
                .WhereIf(searchModel.IsDeleted.HasValue, x => x.IsDeleted == searchModel.IsDeleted.Value)
                .WhereIf(!string.IsNullOrEmpty(searchModel.UserName), x => x.UserName.ToLower().StartsWith(searchModel.UserName.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.Email), x => x.Email.ToLower().StartsWith(searchModel.Email.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.FirstName), x => x.FirstName.ToLower().StartsWith(searchModel.FirstName.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.LastName), x => x.LastName.ToLower().StartsWith(searchModel.LastName.ToLower()))
                .WhereIf(searchModel.CityIds.IsNotEmpty(), x => searchModel.CityIds.Contains(x.CityId.GetValueOrDefault()))
                .WhereIf(searchModel.RoleIds.IsNotEmpty(), x => searchModel.RoleIds.Any(y => x.UserRoles.Any(z => z.RoleId == y)))
                .WhereIf(searchModel.CompanyIds.IsNotEmpty(), x => searchModel.CompanyIds.Any(y => x.CompanyUsers.Any(z => z.CompanyId == y)))
                .WhereIf(searchModel.BusinessUnitIds.IsNotEmpty(), x => searchModel.BusinessUnitIds.Any(y => x.BusinessUnitUsers.Any(z => z.BusinessUnitId == y)));
        }

        virtual public IQueryable<User> BuildIncludesById(string id, IQueryable<User> query) => query;

        virtual public IQueryable<User> BuildIncludes(IQueryable<User> query) => query;

        virtual public IQueryable<User> BuildQueryOrdering(IQueryable<User> Query, UserSearchModel searchModel = null)
        {
            if (searchModel?.Pagination?.OrderFields == null)
                return Query;

            foreach (var orderField in searchModel.Pagination.OrderFields)
                Query = Query.OrderBy($"{orderField.Field} {orderField.Direction}");

            return Query;
        }

        virtual public IQueryable<User> BuildQueryPagination(IQueryable<User> Query, UserSearchModel searchModel = null)
        {
            if (searchModel?.Pagination == null)
                return Query;

            if (searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                return Query;

            return Query.Skip(searchModel.Pagination.Skip.GetValueOrDefault())
                        .Take(searchModel.Pagination.Take.GetValueOrDefault());
        }

        virtual public async Task<Result<List<UserDTO>>> Get(UserSearchModel searchModel = null)
        {
            var result = await GetEntities(searchModel);

            if (!result.IsSuccess)
                return Result.Failed<List<UserDTO>>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<List<UserDTO>>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<List<UserDTO>>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<List<User>>> GetEntities(UserSearchModel searchModel = null)
        {
            try
            {
                Query = BuildQueryConditions(Query, searchModel);
                Query = BuildIncludes(Query);
                Query = BuildQueryOrdering(Query, searchModel);

                if (searchModel?.Pagination != null)
                    searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

                if (searchModel?.Pagination != null && !searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                    Query = BuildQueryPagination(Query, searchModel);

                var items = await Query.AsNoTracking().ToListAsync();

                return Result.Success(items);
            }
            catch (Exception e)
            {
                return Result.Failed<List<User>>("Failed to load entities");
            }
        }

        virtual public async Task<Result<UserDTO>> GetById(string id)
        {
            var result = await GetEntityById(id);

            if (!result.IsSuccess)
                return Result.Failed<UserDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<UserDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<UserDTO>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<User>> GetEntityById(string id)
        {
            try
            {
                var query = DatabaseContext.Users.Where(x => x.Id.Equals(id));

                query = BuildIncludesById(id, query);

                var item = await query.AsNoTracking().FirstOrDefaultAsync();

                return Result.Success(item);
            }
            catch (Exception e)
            {
                return Result.Failed<User>("Failed to load entity");
            }
        }

        virtual public async Task<Result<User>> GetEntityByIdWithoutIncludes(string id)
        {
            try
            {
                var query = DatabaseContext.Users.Where(x => x.Id.Equals(id));

                var item = await query.AsNoTracking().FirstOrDefaultAsync();

                return Result.Success(item);
            }
            catch (Exception e)
            {
                return Result.Failed<User>("Failed to load entity");
            }
        }

        virtual public async Task<Result<UserDTO>> Add(User entity)
        {
            var result = await AddEntity(entity);

            if (!result.IsSuccess)
                return Result.Failed<UserDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<UserDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<UserDTO>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<User>> AddEntity(User entity)
        {
            try
            {
                entity.Id ??= Guid.NewGuid().ToString();

                var password = PasswordGeneratorService.Generate();

                var result = await AddEntity(entity, password);

                if (result.IsSuccess)
                {
                    await EmailService.Send(entity.Email,
                                      "Your account info",
                                      $"UserName: {entity.UserName}\nPassword: {password}");
                }

                return result;
            }
            catch (Exception e)
            {
                return Result.Failed<User>("Failed to add entity");
            }
        }

        virtual public async Task<Result<UserDTO>> Update(User entity)
        {
            var result = await UpdateEntity(entity);

            if (!result.IsSuccess)
                return Result.Failed<UserDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<UserDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<UserDTO>("Failed to map Entity to DTO");
            }
        }

        protected async Task<string> GenerateRandomPassword()
        {
            var password = "";

            return password;
        }

        virtual public async Task<Result<User>> UpdateEntity(User entity)
        {
            try
            {
                DatabaseContext.Users.Update(entity);
                await DatabaseContext.SaveChangesAsync();

                return Result.Success(entity);
            }
            catch (Exception e)
            {
                return Result.Failed<User>("Failed to update entity");
            }
        }

        virtual public async Task<Result> Delete(string id, bool softDelete = true)
        {
            var item = await DatabaseContext.Set<User>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            return await Delete(item, softDelete);
        }

        virtual public async Task<Result> Delete(User entity, bool softDelete = true)
        {
            try
            {
                if (softDelete)
                {
                    entity.IsDeleted = true;
                    await Update(entity);
                }
                else
                {
                    DatabaseContext.Set<User>().Remove(entity);
                }

                await DatabaseContext.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failed("Failed to delete entity");
            }
        }

        #endregion Base
    }
}
