using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddOrUpdateRange(List<TranslationAddModel> models)
        {
            models = models.Where(x => x.Key.HasValue() && x.Value.HasValue()).ToList();

            var entities = Mapper.Map<List<Translation>>(models);

            await AddOrUpdateRange(entities);

            return ;
        }
    }
}
