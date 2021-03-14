using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface IBaseService<TEntity, TSearchModel, TDTO> :
        IBaseService<TEntity, TSearchModel, TDTO, int>
        where TEntity : IBaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO
    { }

    public interface IBaseService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>
    {
        Task<Result<TDTO>> Add(TEntity entity);
        Task<Result<TEntity>> AddEntity(TEntity entity);
        Task<Result<List<TDTO>>> Get(TSearchModel searchModel = null);
        Task<Result<List<TEntity>>> GetEntities(TSearchModel searchModel = null);
        Task<Result<TDTO>> GetById(TId id);
        Task<Result<TEntity>> GetEntityById(TId id);
        Task<Result<TDTO>> Update(TEntity entity);
        Task<Result<TEntity>> UpdateEntity(TEntity entity);
        Task<Result> Delete(TEntity entity, bool softDelete = true);
        Task<Result> Delete(TId id, bool softDelete = true);
    }
}
