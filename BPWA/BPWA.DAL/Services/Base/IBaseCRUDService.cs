using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IBaseCRUDService<TEntity, TSearchModel, TDTO> :
        IBaseCRUDService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO
    { }

    public interface IBaseCRUDService<TEntity, TSearchModel, TDTO, TId> :
        IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>
    {
        Task<TDTO> Add(TEntity entity);
        Task<TEntity> AddEntity(TEntity entity);
        Task<TDTO> Update(TEntity entity);
        Task<TEntity> UpdateEntity(TEntity entity);
        Task<TEntity> IncludeRelatedEntitiesToDelete(TEntity entity);
        Task Delete(TEntity entity);
        Task Delete(TId id);
    }
}
