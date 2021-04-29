using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IBaseReadNewService<TEntity, TSearchModel, TDTO> :
        IBaseReadNewService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO
    { }

    public interface IBaseReadNewService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO<TId>
    {
        Task<Result<List<TDTO>>> Get(TSearchModel searchModel);
        Task<Result<List<TEntity>>> GetEntities(TSearchModel searchModel);
        Task<Result<TDTO>> GetById(TId id);
        Task<Result<TEntity>> GetEntityById(TId id, bool shouldTranslate = true);
        Task<Result<TEntity>> GetEntityByIdWithoutIncludes(TId id, bool shouldTranslate = true);
    }
}
