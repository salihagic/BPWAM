using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;

namespace BPWA.DAL.Services
{
    public interface ICitiesWebService :
        IBaseWebService<City, CitySearchModel, CityDTO, CityAddModel, CityUpdateModel>,
        ICitiesService
    {
    }
}
