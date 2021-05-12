using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Exceptions;
using BPWA.Common.Extensions;
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
using System.Linq.Expressions;
using System.Reflection;
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
        protected readonly ICurrentUser CurrentUser;
        protected readonly IPasswordGeneratorService PasswordGeneratorService;
        protected readonly IEmailService EmailService;
        protected readonly RouteSettings RouteSettings;

        public UsersService(
            DatabaseContext databaseContext,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICurrentUser currentUser,
            IPasswordGeneratorService passwordGeneratorService,
            IEmailService emailService,
            RouteSettings routeSettings
            )
        {
            DatabaseContext = databaseContext;
            Mapper = mapper;
            Query = databaseContext.Users.AsQueryable().AsNoTracking();
            UserManager = userManager;
            SignInManager = signInManager;
            CurrentUser = currentUser;
            PasswordGeneratorService = passwordGeneratorService;
            EmailService = emailService;
            RouteSettings = routeSettings;
        }

        public async Task<User> AddEntity(User entity, string password)
        {
            await DatabaseContext.Users.AddAsync(entity);
            await DatabaseContext.SaveChangesAsync();

            var result = await UserManager.AddPasswordAsync(entity, password);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(x => x.Description).ToArray());

            return entity;
        }

        #region Base

        virtual public IQueryable<User> BuildQueryConditions(IQueryable<User> Query, UserSearchModel searchModel = null)
        {
            if (searchModel == null)
                return Query;

            return Query
                .WhereIf(!string.IsNullOrEmpty(searchModel?.SearchTerm), x => x.UserName.ToLower().StartsWith(searchModel.SearchTerm.ToLower()) || x.Email.ToLower().StartsWith(searchModel.SearchTerm.ToLower()) || x.FirstName.ToLower().StartsWith(searchModel.SearchTerm.ToLower()) || x.LastName.ToLower().StartsWith(searchModel.SearchTerm.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.UserName), x => x.UserName.ToLower().StartsWith(searchModel.UserName.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.Email), x => x.Email.ToLower().StartsWith(searchModel.Email.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.FirstName), x => x.FirstName.ToLower().StartsWith(searchModel.FirstName.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.LastName), x => x.LastName.ToLower().StartsWith(searchModel.LastName.ToLower()))
                .WhereIf(searchModel.CityIds.IsNotEmpty(), x => searchModel.CityIds.Contains(x.CityId.GetValueOrDefault()))
                .WhereIf(searchModel.RoleIds.IsNotEmpty(), x => searchModel.RoleIds.Any(y => x.UserRoles.Any(z => z.RoleId == y)));
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

        virtual public async Task<List<UserDTO>> Get(UserSearchModel searchModel = null)
        {
            var result = await GetEntities(searchModel);

            try
            {
                var mapped = Mapper.Map<List<UserDTO>>(result);

                return mapped;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<List<User>> GetEntities(UserSearchModel searchModel = null)
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

                return items;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load entities");
            }
        }

        virtual public async Task<UserDTO> GetById(string id, bool shouldTranslate = true, bool includeRelated = true)
        {
            var result = await GetEntityById(id, shouldTranslate, includeRelated);

            try
            {
                var mapped = Mapper.Map<UserDTO>(result);

                return mapped;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<User> GetEntityById(string id, bool shouldTranslate = true, bool includeRelated = true)
        {
            try
            {
                var query = DatabaseContext.Users.Where(x => x.Id.Equals(id));

                if (includeRelated)
                    query = BuildIncludesById(id, query);

                var item = await query.AsNoTracking().FirstOrDefaultAsync();

                return item;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load entity");
            }
        }

        virtual public async Task<UserDTO> Add(User entity)
        {
            var result = await AddEntity(entity);

            try
            {
                var mapped = Mapper.Map<UserDTO>(result);

                return mapped;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<User> AddEntity(User entity)
        {
            try
            {
                entity.Id ??= Guid.NewGuid().ToString();

                var password = PasswordGeneratorService.Generate();

                var result = await AddEntity(entity, password);

                await EmailService.Send(entity.Email,
                                  "Your account info",
                                  $"UserName: {entity.UserName}\nPassword: {password}");

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        virtual public async Task<UserDTO> Update(User entity)
        {
            var result = await UpdateEntity(entity);

            try
            {
                var mapped = Mapper.Map<UserDTO>(result);

                return mapped;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<User> UpdateEntity(User entity)
        {
            try
            {
                DatabaseContext.Users.Update(entity);
                await DatabaseContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to update entity");
            }
        }

        virtual public async Task Delete(string id)
        {
            var item = await DatabaseContext.Set<User>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            await Delete(item);
        }

        virtual public async Task Delete(User entity)
        {
            entity = await IncludeRelatedEntitiesToDelete(entity);

            DatabaseContext.Set<User>().Remove(entity);

            await DatabaseContext.SaveChangesAsync();
        }

        public Task<User> IncludeRelatedEntitiesToDelete(User entity)
        {
            return DatabaseContext.Users
                .Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);
        }

        #endregion Base

        #region Helpers 

        protected async Task ManageRelatedEntities<TConnectionEntity>(
            int entityKey,
            List<int> itemIds,
            Expression<Func<TConnectionEntity, int>> entityKeySelector,
            Expression<Func<TConnectionEntity, int>> relatedEntityKeySelector,
            Expression<Func<TConnectionEntity, bool>> currentRelatedItemsPredicate = null,
            bool ignoreQueryFilters = false
            )
            where TConnectionEntity : class, IBaseEntity, new()
        {
            await ManageRelatedEntities<TConnectionEntity, int, int>(entityKey, itemIds, entityKeySelector, relatedEntityKeySelector, currentRelatedItemsPredicate, ignoreQueryFilters);
        }

        protected async Task ManageRelatedEntities<TConnectionEntity, TRelatedEntityKey>(
            int entityKey,
            List<TRelatedEntityKey> itemIds,
            Expression<Func<TConnectionEntity, int>> entityKeySelector,
            Expression<Func<TConnectionEntity, TRelatedEntityKey>> relatedEntityKeySelector,
            Expression<Func<TConnectionEntity, bool>> currentRelatedItemsPredicate = null,
            bool ignoreQueryFilters = false
            )
            where TConnectionEntity : class, IBaseEntity<int>, new()
        {
            await ManageRelatedEntities<TConnectionEntity, int, TRelatedEntityKey>(entityKey, itemIds, entityKeySelector, relatedEntityKeySelector, currentRelatedItemsPredicate, ignoreQueryFilters);
        }

        /// <summary>
        /// Adds new related entities to n-n table and removes removed related entities from n-n table
        /// </summary>
        /// <typeparam name="TConnectionEntity"></typeparam>
        /// <typeparam name="TEntityKey"></typeparam>
        /// <typeparam name="TRelatedEntityKey"></typeparam>
        /// <param name="entityKey"></param>
        /// <param name="itemIds"></param>
        /// <param name="entityKeySelector"></param>
        /// <param name="relatedEntityKeySelector"></param>
        /// <returns></returns>
        protected async Task ManageRelatedEntities<TConnectionEntity, TEntityKey, TRelatedEntityKey>(
            TEntityKey entityKey,
            List<TRelatedEntityKey> itemIds,
            Expression<Func<TConnectionEntity, TEntityKey>> entityKeySelector,
            Expression<Func<TConnectionEntity, TRelatedEntityKey>> relatedEntityKeySelector,
            Expression<Func<TConnectionEntity, bool>> currentRelatedItemsPredicate = null,
            bool ignoreQueryFilters = false
            )
            where TConnectionEntity : class, IBaseEntity<int>, new()
        {
            var dbSet = DatabaseContext.Set<TConnectionEntity>();
            List<TConnectionEntity> currentRelatedItems = null;
            var query = dbSet.AsQueryable();

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (currentRelatedItemsPredicate != null)
            {
                currentRelatedItems = await query.Where(currentRelatedItemsPredicate).ToListAsync();
            }
            else
            {
                ParameterExpression parameter = Expression.Parameter(typeof(TConnectionEntity));
                var predicate = Expression.Lambda<Func<TConnectionEntity, bool>>(
                    Expression.Equal(
                        Expression.Invoke(entityKeySelector, parameter),
                        Expression.Constant(entityKey)),
                        parameter);

                currentRelatedItems = await query.Where(predicate).ToListAsync();
            }

            //Delete
            var relatedItemsToDelete = currentRelatedItems.Where(x => !itemIds?.Any(y => relatedEntityKeySelector.Compile().Invoke(x).Equals(y)) ?? true).ToList();
            dbSet.RemoveRange(relatedItemsToDelete);

            //Add new ones
            var relatedItemIdsToAdd = itemIds.Where(x => !currentRelatedItems.Any(y => relatedEntityKeySelector.Compile().Invoke(y).Equals(x))).ToList();

            var toAdd = new List<TConnectionEntity>();

            if (relatedItemIdsToAdd.IsNotEmpty())
            {
                foreach (var itemId in relatedItemIdsToAdd)
                {
                    var relatedEntity = new TConnectionEntity();

                    //Set entity Id
                    var entityKeyPropertyName = ((entityKeySelector.Body as MemberExpression).Member as PropertyInfo).Name;
                    var entityKeyProperty = relatedEntity.GetType().GetProperty(entityKeyPropertyName);
                    entityKeyProperty.SetValue(relatedEntity, entityKey);

                    //Set related entity Id
                    var relatedEntityKeyPropertyName = ((relatedEntityKeySelector.Body as MemberExpression).Member as PropertyInfo).Name;
                    var relatedEntityKeyProperty = relatedEntity.GetType().GetProperty(relatedEntityKeyPropertyName);
                    relatedEntityKeyProperty.SetValue(relatedEntity, itemId);

                    toAdd.Add(relatedEntity);
                }
            }

            await dbSet.AddRangeAsync(toAdd);

            await DatabaseContext.SaveChangesAsync();
        }

        #endregion
    }
}
