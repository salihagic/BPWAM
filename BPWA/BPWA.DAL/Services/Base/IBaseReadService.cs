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
        where TDTO : class, IBaseDTO
    { }

    public interface IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>
    {
        Task<List<TDTO>>  Get(TSearchModel searchModel);
        Task<List<TEntity>>  GetEntities(TSearchModel searchModel);
        Task<TDTO> GetById(TId id, bool shouldTranslate = true, bool includeRelated = true);
        Task<TEntity> GetEntityById(TId id, bool shouldTranslate = true, bool includeRelated = false);
    }
}
