using AutoMapper;
using BPWA.Common.Exceptions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class BaseTranslatableCRUDService<TEntity, TSearchModel, TDTO>
        : BaseTranslatableCRUDService<TEntity, TSearchModel, TDTO, int>,
          IBaseCRUDService<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO
    {
        public BaseTranslatableCRUDService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, translationsService) { }
    }

    public class BaseTranslatableCRUDService<TEntity, TSearchModel, TDTO, TId> :
        BaseTranslatableReadService<TEntity, TSearchModel, TDTO, TId>,
        IBaseCRUDService<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>
    {
        public BaseTranslatableCRUDService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, translationsService)
        {
        }

        virtual public async Task<TDTO> Add(TEntity entity)
        {
            var entityResult = await AddEntity(entity);

            try
            {
                return Mapper.Map<TDTO>(entityResult);
            }
            catch (Exception exception)
            {
                throw new MappingException(exception);
            }
        }

        virtual public async Task<TEntity> AddEntity(TEntity entity)
        {
            await DatabaseContext.Set<TEntity>().AddAsync(entity);
            await DatabaseContext.SaveChangesAsync();

            return entity;
        }

        virtual public async Task<TDTO> Update(TEntity entity)
        {
            var entityResult = await UpdateEntity(entity);

            try
            {
                return Mapper.Map<TDTO>(entityResult);
            }
            catch (Exception exception)
            {
                throw new MappingException(exception);
            }
        }

        virtual public async Task<TEntity> UpdateEntity(TEntity entity)
        {
            DatabaseContext.Set<TEntity>().Update(entity);
            await DatabaseContext.SaveChangesAsync();

            return entity;
        }

        virtual public async Task Delete(TEntity entity)
        {
            DatabaseContext.Set<TEntity>().Remove(entity);

            await DatabaseContext.SaveChangesAsync();
        }

        virtual public async Task Delete(TId id)
        {
            var item = await DatabaseContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            await Delete(item);
        }

        virtual public async Task<TEntity> IncludeRelatedEntitiesToDelete(TEntity entity) => entity;
    }
}
