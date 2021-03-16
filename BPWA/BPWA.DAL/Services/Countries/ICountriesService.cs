using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ICountriesService : IBaseCRUDService<Country, CountrySearchModel, CountryDTO>
    {
    }
}
