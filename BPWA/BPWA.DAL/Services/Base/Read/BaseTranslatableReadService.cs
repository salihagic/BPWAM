using AutoMapper;
using BPWA.Common.Exceptions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class BaseTranslatableReadService<TEntity, TSearchModel, TDTO>
        : BaseTranslatableReadService<TEntity, TSearchModel, TDTO, int>,
          IBaseReadService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO
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
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>
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

        public override async Task<TEntity> GetEntityById(TId id, bool shouldTranslate = true, bool includeRelated = false)
        {
            var entity = await base.GetEntityById(id, true, includeRelated);

            try
            {
                if (shouldTranslate)
                    entity = await TranslationsService.Translate(entity);

                return entity;
            }
            catch (Exception exception)
            {
                throw new TranslationException(exception);
            }
        }

        public override async Task<List<TEntity>>  GetEntities(TSearchModel searchModel)
        {
            var entities = await base.GetEntities(searchModel);

            try
            {
                entities = await TranslationsService.Translate(entities);

                return entities;
            }
            catch (Exception exception)
            {
                throw new TranslationException(exception);
            }
        }
    }
}
