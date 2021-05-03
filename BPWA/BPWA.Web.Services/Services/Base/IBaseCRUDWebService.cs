using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Threading.Tasks;
using System;

namespace BPWA.Web.Services.Services
{
    public interface IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel> :
        IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO, new()
        where TAddModel : class, new()
        where TUpdateModel : class, IBaseUpdateModel, new()
    { }

    public interface IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> :
        IBaseCRUDService<TEntity, TSearchModel, TDTO, TId>,
        IBaseReadWebService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>, new()
        where TAddModel : class, new()
        where TUpdateModel : class, IBaseUpdateModel<TId>, new()
    {
        virtual async Task<TDTO> Add(TAddModel model) => throw new NotImplementedException();
        virtual async Task<TUpdateModel> PrepareForUpdate(TId id) => throw new NotImplementedException();
        virtual async Task<TDTO> Update(TUpdateModel model) => throw new NotImplementedException();
    }
}
