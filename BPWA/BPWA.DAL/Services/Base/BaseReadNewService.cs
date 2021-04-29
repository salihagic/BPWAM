using AutoMapper;
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
    public class BaseReadNewService<TEntity, TSearchModel, TDTO>
        : BaseReadNewService<TEntity, TSearchModel, TDTO, int>,
          IBaseReadNewService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO
    {
        public BaseReadNewService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }
    }

    public class BaseReadNewService<TEntity, TSearchModel, TDTO, TId> :
        IBaseReadNewService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO<TId>
    {
        protected DatabaseContext DatabaseContext { get; set; }
        protected IQueryable<TEntity> Query { get; set; }
        protected IMapper Mapper { get; set; }
        public bool _shouldTranslate { get; set; }

        public BaseReadNewService(
            DatabaseContext databaseContext,
            IMapper mapper,
            bool shouldTranslate = false
            )
        {
            DatabaseContext = databaseContext;
            Mapper = mapper;
            Query = databaseContext.Set<TEntity>().AsQueryable();
            _shouldTranslate = shouldTranslate;
        }

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

        virtual public async Task<Result<List<TDTO>>> Get(TSearchModel searchModel)
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

        virtual public async Task<Result<List<TEntity>>> GetEntities(TSearchModel searchModel)
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

        virtual public async Task<Result<TEntity>> GetEntityById(TId id, bool shouldTranslate = true)
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

        virtual public async Task<Result<TEntity>> GetEntityByIdWithoutIncludes(TId id, bool shouldTranslate = true)
        {
            try
            {
                var query = DatabaseContext.Set<TEntity>().Where(x => x.Id.Equals(id));

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
