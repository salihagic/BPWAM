using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface IBaseCRUDService<TEntity, TSearchModel, TDTO> :
        IBaseCRUDService<TEntity, TSearchModel, TDTO, int>
        where TEntity : IBaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO
    { }

    public interface IBaseCRUDService<TEntity, TSearchModel, TDTO, TId> :
        IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>
    {
        Task<Result<TDTO>> Add(TEntity entity);
        Task<Result<TEntity>> AddEntity(TEntity entity);
        Task<Result<TDTO>> Update(TEntity entity);
        Task<Result<TEntity>> UpdateEntity(TEntity entity);
        Task<Result> Delete(TEntity entity, bool softDelete = true);
        Task<Result> Delete(TId id, bool softDelete = true);
    }
}
