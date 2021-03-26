using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface IBaseReadService<TEntity, TSearchModel, TDTO> :
        IBaseReadService<TEntity, TSearchModel, TDTO, int>
        where TEntity : IBaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO
    { }

    public interface IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>
    {
        Task<Result<List<TDTO>>> Get(TSearchModel searchModel = null);
        Task<Result<List<TEntity>>> GetEntities(TSearchModel searchModel = null);
        Task<Result<TDTO>> GetById(TId id);
        Task<Result<TEntity>> GetEntityById(TId id);
        Task<Result<TEntity>> GetEntityByIdWithoutIncludes(TId id);
    }
}
