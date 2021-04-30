using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class BaseTranslatableReadService<TEntity, TSearchModel, TDTO>
        : BaseTranslatableReadService<TEntity, TSearchModel, TDTO, int>,
          IBaseReadService<TEntity, TSearchModel, TDTO, int>
        where TEntity : BaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO
    {
        public BaseTranslatableReadService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, translationsService) { }
    }

    public class BaseTranslatableReadService<TEntity, TSearchModel, TDTO, TId> :
        BaseReadService<TEntity, TSearchModel, TDTO, TId>,
        IBaseReadService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : BaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>
    {
        protected ITranslationsService TranslationsService;

        public BaseTranslatableReadService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper)
        {
            TranslationsService = translationsService;
        }

        public override async Task<Result<TEntity>> GetEntityById(TId id, bool shouldTranslate = true, bool includeRelated = false)
        {
            var result = await base.GetEntityById(id, true, includeRelated);

            if (result.IsSuccess && shouldTranslate)
                result.Item = await TranslationsService.Translate(result.Item);

            return result;
        }

        //public override async Task<Result<TEntity>> GetEntityByIdWithoutIncludes(TId id, bool shouldTranslate = true)
        //{
        //    var result = await base.GetEntityByIdWithoutIncludes(id);

        //    if (result.IsSuccess && shouldTranslate)
        //        result.Item = await TranslationsService.Translate(result.Item);

        //    return result;
        //}

        public override async Task<Result<List<TEntity>>> GetEntities(TSearchModel searchModel = null)
        {
            var result = await base.GetEntities(searchModel);

            if (result.IsSuccess)
                result.Item = await TranslationsService.Translate(result.Item);

            return result;
        }
    }
}
