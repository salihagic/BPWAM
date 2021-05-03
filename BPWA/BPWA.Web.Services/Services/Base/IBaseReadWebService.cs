using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;

namespace BPWA.Web.Services.Services
{
    public interface IBaseReadWebService<TEntity, TSearchModel, TDTO> :
        IBaseReadWebService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO, new()
    { }

    public interface IBaseReadWebService<TEntity, TSearchModel, TDTO, TId> :
        IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>, new()
    {
    }
}
