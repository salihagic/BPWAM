using AutoMapper;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Company.RolesManagement)]
    public class RolesController :
        BaseCRUDController<
            Role,
            RoleSearchModel,
            RoleDTO,
            RoleAddModel,
            RoleUpdateModel,
            string
            >
    {
        public RolesController(
            IRolesWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, mapper, toast)
        { }
    }
}
