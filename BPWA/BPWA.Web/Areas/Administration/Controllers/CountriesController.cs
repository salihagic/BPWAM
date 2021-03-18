using AutoMapper;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Helpers.Routing;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace BPWA.Administration.Controllers
{
    [Area(Areas.Administration)]
    [Authorize(Policy = AppClaims.Authorization.CountriesManagement)]
    public class CountriesController :
        BaseCRUDController<
            Country,
            CountrySearchModel,
            CountryDTO,
            CountryAddModel,
            CountryUpdateModel
            >
    {
        public CountriesController(
            ICountriesWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, toast, mapper)
        { }
    }
}
