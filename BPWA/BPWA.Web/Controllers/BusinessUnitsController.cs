using AutoMapper;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Company.BusinessUnitsManagement)]
    public class BusinessUnitsController :
        BaseCRUDController<
            BusinessUnit,
            BusinessUnitSearchModel,
            BusinessUnitDTO,
            BusinessUnitAddModel,
            BusinessUnitUpdateModel
            >
    {
        public BusinessUnitsController(
            IBusinessUnitsWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, mapper, toast)
        { }
    }
}
