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
            var result = await _usersWebService.SendPasswordResetToken(userId);

            if (!result.IsSuccess)
                return BadRequest();

            return Ok();
        }

        #region Edit roles

        public async Task<IActionResult> EditUserRoles()
        {
            var result = await _usersWebService.PrepareForUpdateUserRoles();

            if (!result.IsSuccess)
                return Error();

            return View(result.Item);
        }

        //public async Task<IActionResult> GetRolesForEdit()
        //{
        //    var result = await BaseCRUDService.PrepareForAdd();

        //    if (!result.IsSuccess)
        //        return fullPage ? Error() : _Error();

        //    var model = result.Item;

        //    return View(model);
        //}

        //[HttpPost, Transaction]
        //public virtual async Task<IActionResult> Add(TAddModel model)
        //{
        //    ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

        //    async Task<IActionResult> Failed()
        //    {
        //        var result = await BaseCRUDService.PrepareForAdd(model);

        //        if (!result.IsSuccess)
        //            return Error();

        //        return View(result.Item);
        //    }

        //    if (!ModelState.IsValid)
        //        return await Failed();

        //    try
        //    {
        //        var entityResult = await BaseCRUDService.MapAddModelToEntity(model);

        //        Result<TDTO> result = null;

        //        if (!entityResult.IsSuccess)
        //        {
        //            var entity = Mapper.Map<TEntity>(model);
        //            result = await BaseCRUDService.Add(entity);
        //        }
        //        else
        //        {
        //            var entity = entityResult.Item;
        //            result = await BaseCRUDService.Add(entity);
        //        }

        //        if (!result.IsSuccess)
        //        {
        //            Toast.AddErrorToastMessage(result.GetErrorMessages().FirstOrDefault());
        //            return await Failed();
        //        }

        //        Toast.AddSuccessToastMessage(Message_add_success);
        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        Toast.AddErrorToastMessage(Message_add_error);
        //    }


        //    return await Failed();
        //}

        #endregion
    }
}
