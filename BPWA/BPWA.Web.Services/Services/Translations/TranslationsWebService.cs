using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class TranslationsWebService : TranslationsService, ITranslationsWebService
    {
        public TranslationsWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            IMemoryCache memoryCache
            ) : base(databaseContext, mapper, memoryCache)
        {
        }

        public async Task<Result> AddRange(List<TranslationAddModel> models)
        {
            var entities = Mapper.Map<List<Translation>>(models);

            foreach (var entity in entities)
            {
                var result = await base.AddEntity(entity);

                if (!result.IsSuccess)
                    return result;
            }

            return Result.Success();
        }
    }
}
