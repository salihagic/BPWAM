using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ICountriesWebService :
        IBaseCRUDWebService<Country, CountrySearchModel, CountryDTO, CountryAddModel, CountryUpdateModel>,
        ICountriesService
    {
    }
}
