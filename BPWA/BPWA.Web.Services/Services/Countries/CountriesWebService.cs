using AutoMapper;
using BPWA.Common.Extensions;
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

        public async Task<Result<CountryDTO>> Add(CountryAddModel model)
        {
            return await base.Add(Mapper.Map<Country>(model));
        }

        public async Task<Result<CountryUpdateModel>> PrepareForUpdate(int id)
        {
            var result = await GetEntityById(id, shouldTranslate: false);

            if (!result.IsSuccess)
                return Result.Failed<CountryUpdateModel>(result.FirstErrorMessage());

            return Result.Success(Mapper.Map<CountryUpdateModel>(result.Item));
        }

        public async Task<Result<CountryDTO>> Update(CountryUpdateModel model)
        {
            var result = await GetEntityById(model.Id, shouldTranslate: false);

            if (!result.IsSuccess)
                return Result.Failed<CountryDTO>(result.FirstErrorMessage());

            var entity = result.Item;
            Mapper.Map(model, entity);

            var currentCountryCurrencies = await DatabaseContext.CountryCurrencies.Where(x => x.CountryId == entity.Id).ToListAsync();

            if (currentCountryCurrencies.IsNotEmpty())
            {
                //Delete
                var countryCurrenciesToDelete = currentCountryCurrencies.Where(x => !entity.CountryCurrencies?.Any(y => y.CurrencyId == x.CurrencyId) ?? true).ToList();
                DatabaseContext.RemoveRange(countryCurrenciesToDelete);
                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.CountryCurrencies = entity.CountryCurrencies.Where(x => !currentCountryCurrencies.Any(y => y.CurrencyId == x.CurrencyId)).ToList();
            }

            var currentCountryLanguages = await DatabaseContext.CountryLanguages.Where(x => x.CountryId == entity.Id).ToListAsync();

            if (currentCountryLanguages.IsNotEmpty())
            {
                //Delete
                var countryLanguagesToDelete = currentCountryLanguages.Where(x => !entity.CountryLanguages?.Any(y => y.LanguageId == x.LanguageId) ?? true).ToList();
                DatabaseContext.RemoveRange(countryLanguagesToDelete);
                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.CountryLanguages = entity.CountryLanguages.Where(x => !currentCountryLanguages.Any(y => y.LanguageId == x.LanguageId)).ToList();
            }

            return await base.Update(entity);
        }
    }
}
