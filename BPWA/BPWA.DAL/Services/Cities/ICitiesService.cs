using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ICitiesService : IBaseCRUDService<City, CitySearchModel, CityDTO>
    {
    }
}
