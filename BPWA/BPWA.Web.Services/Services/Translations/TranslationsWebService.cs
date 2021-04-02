using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Services;
using Microsoft.Extensions.Caching.Memory;

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
    }
}
