using AutoMapper;
using BPWA.Common.Resources;
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
using System;
using System.Linq;
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
            base(service, toast, mapper)
        {
            _usersWebService = service;
        }

        #region Change password

        public virtual async Task<IActionResult> ResetPassword(string userId)
        {
            return View(new ResetPasswordModel { UserId = userId });
        }

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            async Task<IActionResult> Failed()
            {
                return View(model);
            }

            if (!ModelState.IsValid)
                return await Failed();

            try
            {
                var result = await _usersWebService.ResetPassword(model);

                if (!result.IsSuccess)
                {
                    Toast.AddErrorToastMessage(result.GetErrorMessages().FirstOrDefault());
                    return await Failed();
                }

                Toast.AddSuccessToastMessage(Translations.Successfully_changed_password);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Toast.AddErrorToastMessage(Translations.Failed_to_change_password);
            }


            return await Failed();
        }

        #endregion
    }
}
