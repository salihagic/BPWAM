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
    [Authorize(Policy = AppPolicies.RolesManagement)]
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
            IRolesWebService rolesWebService,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(rolesWebService, toast, mapper)
        { }
    }
}
