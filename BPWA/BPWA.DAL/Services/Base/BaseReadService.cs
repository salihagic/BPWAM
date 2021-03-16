using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using TFM.DAL.Models;
using System;

namespace BPWA.DAL.Services
{
    public class BaseReadService<TEntity, TSearchModel, TDTO>
        : BaseReadService<TEntity, TSearchModel, TDTO, int>,
          IBaseReadService<TEntity, TSearchModel, TDTO, int>
        where TEntity : BaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO
    {
        public BaseReadService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }
    }    
    
    public class BaseReadService<TEntity, TSearchModel, TDTO, TId> :
        IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : BaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>
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
            Mapper = mapper;
            Query = databaseContext.Set<TEntity>().AsQueryable();
        }

        virtual public IQueryable<TEntity> BuildQueryConditions(IQueryable<TEntity> Query, TSearchModel searchModel = null)
        {
            if (searchModel == null)
                return Query;

            return Query
                .WhereIf(searchModel.IsDeleted.HasValue, x => x.IsDeleted == searchModel.IsDeleted.Value);
        }

        virtual public IQueryable<TEntity> BuildIncludesById(TId id, IQueryable<TEntity> query) => query;

        virtual public IQueryable<TEntity> BuildIncludes(IQueryable<TEntity> query) => query;

        virtual public IQueryable<TEntity> BuildQueryOrdering(IQueryable<TEntity> Query, TSearchModel searchModel = null)
        {
            if (searchModel?.Pagination?.OrderFields == null)
                return Query;

            foreach (var orderField in searchModel.Pagination.OrderFields)
                Query = Query.OrderBy($"{orderField.Field} {orderField.Direction}");

            return Query;
        }

        virtual public IQueryable<TEntity> BuildQueryPagination(IQueryable<TEntity> Query, TSearchModel searchModel = null)
        {
            if (searchModel?.Pagination == null)
                return Query;

            if (searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                return Query;

            return Query.Skip(searchModel.Pagination.Skip.GetValueOrDefault())
                        .Take(searchModel.Pagination.Take.GetValueOrDefault());
        }

        virtual public async Task<Result<List<TDTO>>> Get(TSearchModel searchModel = null)
        {
            var result = await GetEntities(searchModel);

            if (!result.IsSuccess)
                return Result.Failed<List<TDTO>>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<List<TDTO>>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<List<TDTO>>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<List<TEntity>>> GetEntities(TSearchModel searchModel = null)
        {
            try
            {
                Query = BuildQueryConditions(Query, searchModel);
                Query = BuildQueryOrdering(Query, searchModel);

                if (searchModel?.Pagination != null)
                    searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

                if (searchModel?.Pagination != null && !searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                    Query = BuildQueryPagination(Query, searchModel);

                var items = await Query.AsNoTracking().ToListAsync();

                return Result.Success(items);
            }
            catch(Exception e)
            {
                return Result.Failed<List<TEntity>>("Failed to load entities");
            }
        }

        virtual public async Task<Result<TDTO>> GetById(TId id)
        {
            var result = await GetEntityById(id);

            if (!result.IsSuccess)
                return Result.Failed<TDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<TDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<TDTO>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<TEntity>> GetEntityById(TId id)
        {
            try
            {
                var query = DatabaseContext.Set<TEntity>().Where(x => x.Id.Equals(id));

                query = BuildIncludesById(id, query);

                var item = await query.AsNoTracking().FirstOrDefaultAsync();

                return Result.Success(item);
            }
            catch (Exception e)
            {
                return Result.Failed<TEntity>("Failed to load entity");
            }
        }
    }
}
