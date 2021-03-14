using AutoMapper;
using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BPWA.Controllers
{
    [Authorize(Policy = AppPolicies.CitiesManagement)]
    public class CitiesController :
        BaseCRUDController<
            City,
            CitySearchModel,
            CityDTO,
            CityAddModel,
            CityUpdateModel
            >
    {
        public CitiesController(
            ICitiesWebService citiesWebService,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(citiesWebService, toast, mapper)
        { }
    }
}
