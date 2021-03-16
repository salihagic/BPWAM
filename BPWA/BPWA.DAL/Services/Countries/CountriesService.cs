using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class CountriesService : BaseCRUDService<Country, CountrySearchModel, CountryDTO>, ICountriesService
    {
        public CountriesService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }

        public override IQueryable<Country> BuildQueryConditions(IQueryable<Country> query, CountrySearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.Name.ToLower().StartsWith(searchModel.Name.ToLower()))
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Code), x => x.Code.ToLower().StartsWith(searchModel.Code.ToLower()))
                       .WhereIf(searchModel.CurrencyIds.IsNotEmpty(), x => x.CountryCurrencies.Any(y => searchModel.CurrencyIds.Contains(y.CurrencyId) && !y.IsDeleted))
                       .WhereIf(searchModel.LanguageIds.IsNotEmpty(), x => x.CountryLanguages.Any(y => searchModel.LanguageIds.Contains(y.LanguageId) && !y.IsDeleted));
        }
    }
}
