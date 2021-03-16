using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.Web.Services.Services
{
    public interface IBaseReadWebService<TEntity, TSearchModel, TDTO> :
        IBaseReadWebService<TEntity, TSearchModel, TDTO, int>
        where TEntity : IBaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO, new()
    { }

    public interface IBaseReadWebService<TEntity, TSearchModel, TDTO, TId> :
        IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>, new()
    {
        async Task<Result<TSearchModel>> PrepareForGet(TSearchModel searchModel = null) => Result.Success(searchModel ?? new TSearchModel { IsDeleted = false });
    }
}
