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
    [Authorize(Policy = AppClaims.Authorization.Administration.UsersManagement)]
    public class UsersController :
        BaseCRUDController<
            User,
            UserSearchModel,
            UserDTO,
            UserAddModel,
            UserUpdateModel,
            string
            >
    {
        private IUsersWebService _usersWebService;

        public UsersController(
            IUsersWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, mapper, toast)
        {
            _usersWebService = service;
        }

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> SendPasswordResetToken(string userId)
        {
            await _usersWebService.SendPasswordResetToken(userId);

            return Ok();
        }
    }
}
