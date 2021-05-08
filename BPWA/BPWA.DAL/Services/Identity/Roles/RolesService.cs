using AutoMapper;
using BPWA.Common.Extensions;
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
using System.Linq.Expressions;
using System.Reflection;

namespace BPWA.DAL.Services
{
    public class RolesService : IRolesService
    {
        protected DatabaseContext DatabaseContext { get; set; }
        protected IQueryable<Role> Query { get; set; }
        protected IMapper Mapper { get; set; }
        protected RoleManager<Role> RoleManager { get; set; }

        public RolesService(
            DatabaseContext databaseContext,
            IMapper mapper,
            RoleManager<Role> roleManager
            )
        {
            DatabaseContext = databaseContext;
            Mapper = mapper;
            Query = databaseContext.Set<Role>().AsQueryable();
            RoleManager = roleManager;
        }

        public async Task<Role> GetEntityWithClaimsByName(string name)
        {
            var role = await RoleManager.FindByNameAsync(name);

            if (role == null)
                return null;

            role.RoleClaims = DatabaseContext.RoleClaims.Where(x => x.RoleId == role.Id).ToList();

            return role;
        }

        virtual public async Task<RoleDTO> Add(Role entity)
        {
            var result = await AddEntity(entity);

            try
            {
                var mapped = Mapper.Map<RoleDTO>(result);

                return mapped;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Role> AddEntity(Role entity)
        {
            var exists = await RoleManager.RoleExistsAsync(entity.Name);

            if (exists)
                throw new Exception("Role already exists");

            if (entity.Id == null)
                entity.Id = Guid.NewGuid().ToString();

            var identityResult = await RoleManager.CreateAsync(entity);

            if (!identityResult.Succeeded)
                throw new Exception(identityResult.Errors.First().Description);

            return entity;
        }

        virtual public IQueryable<Role> BuildQueryConditions(IQueryable<Role> Query, RoleSearchModel searchModel = null)
        {
            if (searchModel == null)
                return Query;

            return Query
                .WhereIf(!string.IsNullOrEmpty(searchModel?.SearchTerm), x => x.Name.ToLower().StartsWith(searchModel.SearchTerm.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.Name.ToLower().StartsWith(searchModel.Name.ToLower()))
                .WhereIf(searchModel.Claims.IsNotEmpty(), x => x.RoleClaims.Any(y => searchModel.Claims.Contains(y.ClaimValue)));
        }

        virtual public IQueryable<Role> BuildIncludesById(string id, IQueryable<Role> query) => query;

        virtual public IQueryable<Role> BuildIncludes(IQueryable<Role> query) => query;

        virtual public IQueryable<Role> BuildQueryOrdering(IQueryable<Role> Query, RoleSearchModel searchModel = null)
        {
            if (searchModel?.Pagination?.OrderFields == null)
                return Query;

            foreach (var orderField in searchModel.Pagination.OrderFields)
                Query = Query.OrderBy($"{orderField.Field} {orderField.Direction}");

            return Query;
        }

        virtual public IQueryable<Role> BuildQueryPagination(IQueryable<Role> Query, RoleSearchModel searchModel = null)
        {
            if (searchModel?.Pagination == null)
                return Query;

            if (searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                return Query;

            return Query.Skip(searchModel.Pagination.Skip.GetValueOrDefault())
                        .Take(searchModel.Pagination.Take.GetValueOrDefault());
        }

        virtual public async Task<List<RoleDTO>> Get(RoleSearchModel searchModel = null)
        {
            var result = await GetEntities(searchModel);

            try
            {
                var mapped = Mapper.Map<List<RoleDTO>>(result);

                return mapped;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<List<Role>> GetEntities(RoleSearchModel searchModel = null)
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

        virtual public async Task<RoleDTO> GetById(string id, bool shouldTranslate = true, bool includeRelated = true)
        {
            var result = await GetEntityById(id, shouldTranslate, includeRelated);

            try
            {
                var mapped = Mapper.Map<RoleDTO>(result);

                return mapped;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Role> GetEntityById(string id, bool shouldTranslate = true, bool includeRelated = true)
        {
            try
            {
                var query = DatabaseContext.Set<Role>().Where(x => x.Id.Equals(id));

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

        virtual public async Task<RoleDTO> Update(Role entity)
        {
            var result = await UpdateEntity(entity);

            try
            {
                var mapped = Mapper.Map<RoleDTO>(result);

                return mapped;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Role> UpdateEntity(Role entity)
        {
            try
            {
                DatabaseContext.Set<Role>().Update(entity);
                await DatabaseContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to update an item");
            }
        }

        virtual public async Task Delete(Role entity)
        {
            entity = await IncludeRelatedEntitiesToDelete(entity);

            DatabaseContext.Set<Role>().Remove(entity);

            await DatabaseContext.SaveChangesAsync();
        }

        virtual public async Task Delete(string id)
        {
            var item = await DatabaseContext.Set<Role>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            await Delete(item);
        }

        public Task<Role> IncludeRelatedEntitiesToDelete(Role entity)
        {
            return DatabaseContext.Roles
                .Include(x => x.RoleClaims)
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);
        }

        #region Helpers 

        /// <summary>
        /// Adds new related entities to n-n table and removes removed related entities from n-n table
        /// </summary>
        /// <typeparam name="TConnectionEntity"></typeparam>
        /// <param name="entityKey"></param>
        /// <param name="itemIds"></param>
        /// <param name="entityKeySelector"></param>
        /// <param name="relatedEntityKeySelector"></param>
        /// <returns></returns>
        protected async Task ManageRelatedEntities<TConnectionEntity>(
            int entityKey,
            List<int> itemIds,
            Expression<Func<TConnectionEntity, int>> entityKeySelector,
            Expression<Func<TConnectionEntity, int>> relatedEntityKeySelector
            )
            where TConnectionEntity : class, IBaseEntity, new()
        {
            await ManageRelatedEntities<TConnectionEntity, int, int>(entityKey, itemIds, entityKeySelector, relatedEntityKeySelector);
        }

        /// <summary>
        /// Adds new related entities to n-n table and removes removed related entities from n-n table
        /// </summary>
        /// <typeparam name="TConnectionEntity"></typeparam>
        /// <typeparam name="TRelatedEntityKey"></typeparam>
        /// <param name="entityKey"></param>
        /// <param name="itemIds"></param>
        /// <param name="entityKeySelector"></param>
        /// <param name="relatedEntityKeySelector"></param>
        /// <returns></returns>
        protected async Task ManageRelatedEntities<TConnectionEntity, TRelatedEntityKey>(
            int entityKey,
            List<TRelatedEntityKey> itemIds,
            Expression<Func<TConnectionEntity, int>> entityKeySelector,
            Expression<Func<TConnectionEntity, TRelatedEntityKey>> relatedEntityKeySelector
            )
            where TConnectionEntity : class, IBaseEntity<int>, new()
        {
            await ManageRelatedEntities<TConnectionEntity, int, TRelatedEntityKey>(entityKey, itemIds, entityKeySelector, relatedEntityKeySelector);
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
            Expression<Func<TConnectionEntity, TRelatedEntityKey>> relatedEntityKeySelector
            )
            where TConnectionEntity : class, IBaseEntity<int>, new()
        {
            var dbSet = DatabaseContext.Set<TConnectionEntity>();

            ParameterExpression parameter = Expression.Parameter(typeof(TConnectionEntity));
            var predicate = Expression.Lambda<Func<TConnectionEntity, bool>>(
                Expression.Equal(
                    Expression.Invoke(entityKeySelector, parameter),
                    Expression.Constant(entityKey)),
                    parameter);

            var currentRelatedItems = await dbSet
                .Where(predicate)
                .ToListAsync();

            //Delete
            var relatedItemsToDelete = currentRelatedItems.Where(x => !itemIds?.Any(y => y.Equals(x.Id)) ?? true).ToList();
            dbSet.RemoveRange(relatedItemsToDelete);

            //Add new ones
            var relatedItemIdsToAdd = itemIds
                .Where(x => !currentRelatedItems.Any(y => y.Id.Equals(x)))
                .ToList();

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
