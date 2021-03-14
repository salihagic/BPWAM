using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ICountriesService : IBaseService<Country, CountrySearchModel, CountryDTO>
    {
    }
}
