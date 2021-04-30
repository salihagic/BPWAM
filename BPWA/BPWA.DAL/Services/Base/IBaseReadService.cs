using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IBaseReadService<TEntity, TSearchModel, TDTO> :
        IBaseReadService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : IBaseDTO
    { }

    public interface IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO<TId>
    {
        Task<Result<List<TDTO>>> Get(TSearchModel searchModel);
        Task<Result<List<TEntity>>> GetEntities(TSearchModel searchModel);
        Task<Result<TDTO>> GetById(TId id);
        Task<Result<TEntity>> GetEntityById(TId id, bool shouldTranslate = true, bool includeRelated = false);
        //Task<Result<TEntity>> GetEntityByIdWithoutIncludes(TId id, bool shouldTranslate = true);
    }
}
