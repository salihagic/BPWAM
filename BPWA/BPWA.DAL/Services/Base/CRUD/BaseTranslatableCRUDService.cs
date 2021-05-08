using AutoMapper;
using BPWA.Common.Exceptions;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class BaseTranslatableCRUDService<TEntity, TSearchModel, TDTO>
        : BaseTranslatableCRUDService<TEntity, TSearchModel, TDTO, int>,
          IBaseCRUDService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO
    {
        public BaseTranslatableCRUDService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, translationsService) { }
    }

    public class BaseTranslatableCRUDService<TEntity, TSearchModel, TDTO, TId> :
        BaseTranslatableReadService<TEntity, TSearchModel, TDTO, TId>,
        IBaseCRUDService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>
    {
        public BaseTranslatableCRUDService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, translationsService)
        {
        }

        virtual public async Task<TDTO> Add(TEntity entity)
        {
            var entityResult = await AddEntity(entity);

            try
            {
                return Mapper.Map<TDTO>(entityResult);
            }
            catch (Exception exception)
            {
                throw new MappingException(exception);
            }
        }

        virtual public async Task<TEntity> AddEntity(TEntity entity)
        {
            await DatabaseContext.Set<TEntity>().AddAsync(entity);
            await DatabaseContext.SaveChangesAsync();

            return entity;
        }

        virtual public async Task<TDTO> Update(TEntity entity)
        {
            var entityResult = await UpdateEntity(entity);

            try
            {
                return Mapper.Map<TDTO>(entityResult);
            }
            catch (Exception exception)
            {
                throw new MappingException(exception);
            }
        }

        virtual public async Task<TEntity> UpdateEntity(TEntity entity)
        {
            DatabaseContext.Set<TEntity>().Update(entity);
            await DatabaseContext.SaveChangesAsync();

            return entity;
        }

        virtual public async Task Delete(TEntity entity)
        {
            DatabaseContext.Set<TEntity>().Remove(entity);

            await DatabaseContext.SaveChangesAsync();
        }

        virtual public async Task Delete(TId id)
        {
            var item = await DatabaseContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            await Delete(item);
        }

        virtual public async Task<TEntity> IncludeRelatedEntitiesToDelete(TEntity entity) => entity;

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
