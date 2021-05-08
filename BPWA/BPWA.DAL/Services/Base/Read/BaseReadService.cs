using AutoMapper;
using BPWA.Common.Exceptions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class BaseReadService<TEntity, TSearchModel, TDTO>
        : BaseReadService<TEntity, TSearchModel, TDTO, int>,
          IBaseReadService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO
    {
        public BaseReadService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }
    }

    public class BaseReadService<TEntity, TSearchModel, TDTO, TId> :
        IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>
    {
        protected DatabaseContext DatabaseContext { get; set; }
        protected IQueryable<TEntity> Query { get; set; }
        protected IMapper Mapper { get; set; }

        public BaseReadService(
            DatabaseContext databaseContext,
            IMapper mapper
            )
        {
            DatabaseContext = databaseContext;
            Query = databaseContext.Set<TEntity>().AsQueryable();
            Mapper = mapper;
        }

        virtual public async Task<List<TDTO>>  Get(TSearchModel searchModel)
        {
            var entities = await GetEntities(searchModel);

            try
            {
                return Mapper.Map<List<TDTO>>(entities);
            }
            catch (Exception exception)
            {
                throw new MappingException(exception);
            }
        }

        virtual public async Task<List<TEntity>>  GetEntities(TSearchModel searchModel)
        {
            Query = BuildQueryConditions(Query, searchModel);
            Query = BuildIncludes(Query);
            Query = BuildQueryOrdering(Query, searchModel);

            if (searchModel?.Pagination != null)
                searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

            if (searchModel?.Pagination != null && !searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                Query = BuildQueryPagination(Query, searchModel);

            return await Query.AsNoTracking().ToListAsync();
        }

        virtual public async Task<TDTO> GetById(TId id, bool shouldTranslate = true, bool includeRelated = true)
        {
            var entity = await GetEntityById(id, shouldTranslate, includeRelated);

            try
            {
                return Mapper.Map<TDTO>(entity);
            }
            catch (Exception exception)
            {
                throw new MappingException(exception);
            }
        }

        virtual public async Task<TEntity> GetEntityById(TId id, bool shouldTranslate = true, bool includeRelated = false)
        {
            var query = DatabaseContext.Set<TEntity>().Where(x => x.Id.Equals(id));

            if (includeRelated)
                query = BuildIncludesById(id, query);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        #region Helpers

        virtual public IQueryable<TEntity> BuildQueryConditions(IQueryable<TEntity> Query, TSearchModel searchModel) => Query;

        virtual public IQueryable<TEntity> BuildIncludesById(TId id, IQueryable<TEntity> query) => query;

        virtual public IQueryable<TEntity> BuildIncludes(IQueryable<TEntity> query) => query;

        virtual public IQueryable<TEntity> BuildQueryOrdering(IQueryable<TEntity> Query, TSearchModel searchModel)
        {
            if (searchModel?.Pagination?.OrderFields == null)
                return Query;

            foreach (var orderField in searchModel.Pagination.OrderFields)
                Query = Query.OrderBy($"{orderField.Field} {orderField.Direction}");

            return Query;
        }

        virtual public IQueryable<TEntity> BuildQueryPagination(IQueryable<TEntity> Query, TSearchModel searchModel)
        {
            if (searchModel?.Pagination == null)
                return Query;

            if (searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                return Query;

            return Query.Skip(searchModel.Pagination.Skip.GetValueOrDefault())
                        .Take(searchModel.Pagination.Take.GetValueOrDefault());
        }

        #endregion
    }
}

