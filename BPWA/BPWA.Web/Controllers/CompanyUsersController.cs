using AutoMapper;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Helpers.Filters;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Threading.Tasks;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Company.CompanyUsersManagement)]
    public class CompanyUsersController :
        BaseCRUDController<
            CompanyUser,
            CompanyUserSearchModel,
            CompanyUserDTO,
            CompanyUserAddModel,
            CompanyUserUpdateModel
            >
    {
        private IUsersWebService _usersWebService;

        public CompanyUsersController(
            ICompanyUsersWebService service,
            IUsersWebService usersWebService,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, mapper, toast)
        {
            _usersWebService = usersWebService;
        }

        #region Change password

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> SendPasswordResetToken(string userId)
        {
            var result = await _usersWebService.SendPasswordResetToken(userId);

            if (!result.IsSuccess)
                return BadRequest();

            return Ok();
        }

        #endregion
    }
}
