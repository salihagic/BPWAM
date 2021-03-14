using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.Web.Services.Services
{
    public interface IBaseWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel> :
        IBaseWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, int>
        where TEntity : IBaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO, new()
        where TAddModel : class, new()
        where TUpdateModel : BaseUpdateModel<int>, new()
    { }

    public interface IBaseWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> :
        IBaseService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>, new()
        where TAddModel : class, new()
        where TUpdateModel : BaseUpdateModel<TId>, new()
    {
        async Task<Result<TAddModel>> PrepareForAdd(TAddModel model = null) => Result.Success(model ?? new TAddModel());
        async Task<Result<TEntity>> MapAddModelToEntity(TAddModel model) => Result.Failed<TEntity>("Not implemented");
        async Task<Result<TSearchModel>> PrepareForGet(TSearchModel searchModel = null) => Result.Success(searchModel ?? new TSearchModel { IsDeleted = false });
        async Task<Result<TUpdateModel>> PrepareForUpdate(TUpdateModel model = null) => Result.Success(model ?? new TUpdateModel());
        async Task<Result<TEntity>> MapUpdateModelToEntity(TUpdateModel model) => Result.Failed<TEntity>("Not implemented");
    }
}
