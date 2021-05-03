using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class CountriesWebService : CountriesService, ICountriesWebService
    {
        public CountriesWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }

        public override IQueryable<Country> BuildIncludesById(int id, IQueryable<Country> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.CountryCurrencies)
                       .ThenInclude(x => x.Currency)
                       .Include(x => x.CountryLanguages)
                       .ThenInclude(x => x.Language);
        }

        public async Task<CountryDTO> Add(CountryAddModel model)
        {
            var entity = Mapper.Map<Country>(model);
            var result = await base.Add(entity);

            await ManageRelatedEntities<CountryCurrency>(result.Id, model.CurrencyIds, x => x.CountryId, x => x.CurrencyId);
            await ManageRelatedEntities<CountryLanguage>(result.Id, model.LanguageIds, x => x.CountryId, x => x.LanguageId);

            return result;
        }

        public async Task<CountryDTO> Update(CountryUpdateModel model)
        {
            var entity = await GetEntityById(model.Id, false, false);
            Mapper.Map(model, entity);
            var result = await base.Update(entity);

            await ManageRelatedEntities<CountryCurrency>(result.Id, model.CurrencyIds, x => x.CountryId, x => x.CurrencyId);
            await ManageRelatedEntities<CountryLanguage>(result.Id, model.LanguageIds, x => x.CountryId, x => x.LanguageId);

            return result;
        }
    }
}
