using AutoMapper;
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
        where TEntity : BaseEntity, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO
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
        where TEntity : BaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>
    {
        public BaseTranslatableCRUDService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, translationsService)
        {
        }

        virtual public async Task<Result<TDTO>> Add(TEntity entity)
        {
            var result = await AddEntity(entity);

            if (!result.IsSuccess)
                return Result.Failed<TDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<TDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<TDTO>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<TEntity>> AddEntity(TEntity entity)
        {
            try
            {
                await DatabaseContext.Set<TEntity>().AddAsync(entity);
                await DatabaseContext.SaveChangesAsync();

                return Result.Success(entity);
            }
            catch (Exception e)
            {
                return Result.Failed<TEntity>("Failed to add entity");
            }
        }

        virtual public async Task<Result<TDTO>> Update(TEntity entity)
        {
            var result = await UpdateEntity(entity);

            if (!result.IsSuccess)
                return Result.Failed<TDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<TDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<TDTO>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<TEntity>> UpdateEntity(TEntity entity)
        {
            try
            {
                DatabaseContext.Set<TEntity>().Update(entity);
                await DatabaseContext.SaveChangesAsync();

                return Result.Success(entity);
            }
            catch (Exception e)
            {
                return Result.Failed<TEntity>("Failed to update entity");
            }
        }

        virtual public async Task<Result> Delete(TEntity entity)
        {
            try
            {
                DatabaseContext.Set<TEntity>().Remove(entity);

                await DatabaseContext.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failed("Failed to delete entity");
            }
        }

        virtual public async Task<Result> Delete(TId id)
        {
            var item = await DatabaseContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            return await Delete(item);
        }
    }
}
