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
    public class BaseReadRespository<TEntity, TSearchModel, TDTO>
        : BaseReadNewService<TEntity, TSearchModel, TDTO, int>,
          IBaseReadRespository<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO
    {
        public BaseReadRespository(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }
    }

    public class BaseReadNewService<TEntity, TSearchModel, TDTO, TId> :
        IBaseReadRespository<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO<TId>
    {
        protected DatabaseContext DatabaseContext { get; set; }
        protected IQueryable<TEntity> Query { get; set; }
        protected IMapper Mapper { get; set; }

        public BaseReadNewService(
            DatabaseContext databaseContext,
            IMapper mapper
            )
        {
            DatabaseContext = databaseContext;
            Query = databaseContext.Set<TEntity>().AsQueryable();
            Mapper = mapper;
        }

        public Task<List<TDTO>> Get(TSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public Task<TDTO> GetById(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> GetEntities(TSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetEntityById(TId id)
        {
            throw new NotImplementedException();
        }
    }
}
