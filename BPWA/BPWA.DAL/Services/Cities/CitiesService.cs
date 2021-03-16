using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class CitiesService : BaseCRUDService<City, CitySearchModel, CityDTO>, ICitiesService
    {
        public CitiesService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }

        public override IQueryable<City> BuildQueryConditions(IQueryable<City> query, CitySearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.Name.ToLower().StartsWith(searchModel.Name.ToLower()))
                       .WhereIf(searchModel.CountryId.HasValue, x => x.CountryId == searchModel.CountryId.Value);
        }
    }
}
