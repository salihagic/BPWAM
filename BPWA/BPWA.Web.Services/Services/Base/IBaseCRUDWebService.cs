using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Threading.Tasks;
using BPWA.DAL.Models;

namespace BPWA.Web.Services.Services
{
    public interface IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel> :
        IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, int>
        where TEntity : IBaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO, new()
        where TAddModel : class, new()
        where TUpdateModel : BaseUpdateModel<int>, new()
    { }

    public interface IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> :
        IBaseCRUDService<TEntity, TSearchModel, TDTO, TId>,
        IBaseReadWebService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>, new()
        where TAddModel : class, new()
        where TUpdateModel : BaseUpdateModel<TId>, new()
    {
        virtual async Task<Result<TDTO>> Add(TAddModel model) => Result.Failed<TDTO>("Not implemented");
        virtual async Task<Result<TUpdateModel>> PrepareForUpdate(TId id) => Result.Failed<TUpdateModel>("Not implemented");
        virtual async Task<Result<TDTO>> Update(TUpdateModel model) => Result.Failed<TDTO>("Not implemented");

        virtual async Task<Result<TAddModel>> PrepareForAdd(TAddModel model = null) => Result.Success(model ?? new TAddModel());
        virtual async Task<Result<TEntity>> MapAddModelToEntity(TAddModel model) => Result.Failed<TEntity>("Not implemented");
        virtual async Task<Result<TUpdateModel>> PrepareForUpdate(TUpdateModel model = null) => Result.Success(model ?? new TUpdateModel());
        virtual async Task<Result<TEntity>> MapUpdateModelToEntity(TUpdateModel model) => Result.Failed<TEntity>("Not implemented");
    }
}
