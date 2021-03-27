using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Common.Resources;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Helpers.Filters;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Linq;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.Controllers
{
    public class BaseCRUDController<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel> :
        BaseCRUDController<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, int>
        where TEntity : IBaseEntity<int>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<int>, new()
        where TAddModel : class, new()
        where TUpdateModel : BaseUpdateModel, new()
    {
        public BaseCRUDController(
            IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, int> service,
            IToastNotification toast,
            IMapper mapper
            ) : base(service, toast, mapper) { }
    }

    public class BaseCRUDController<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> :
        BaseReadController<TEntity, TSearchModel, TDTO, TId>
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>, new()
        where TAddModel : class, new()
        where TUpdateModel : BaseUpdateModel<TId>, new()
    {
        #region Props

        public IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> BaseCRUDService;
        public IToastNotification Toast;

        #endregion

        #region Messages 

        protected string Message_add_success;
        protected string Message_add_error;
        protected string Message_edit_success;
        protected string Message_edit_error;
        protected string Message_delete_success;
        protected string Message_delete_error;

        #endregion Messages

        #region Constructor

        public BaseCRUDController(
            IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> service,
            IToastNotification toast,
            IMapper mapper
            ) : base(service, mapper)
        {
            BaseCRUDService = service;
            Toast = toast;

            Message_add_success = Translations.Add_success;
            Message_add_error = Translations.Add_error;
            Message_edit_success = Translations.Edit_success;
            Message_edit_error = Translations.Edit_error;
            Message_delete_success = Translations.Delete_success;
            Message_delete_error = Translations.Delete_error;
        }

        #endregion

        #region Add

        public virtual async Task<IActionResult> Add(bool fullPage = false)
        {
            if (fullPage)
                BreadcrumbItem(null, new { fullPage });

            var result = await BaseCRUDService.PrepareForAdd();

            if (!result.IsSuccess)
                return fullPage ? Error() : _Error();

            var model = result.Item;

            return View(model);
        }

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> Add(TAddModel model)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

            async Task<IActionResult> Failed()
            {
                var result = await BaseCRUDService.PrepareForAdd(model);

                if (!result.IsSuccess)
                    return Error();

                return View(result.Item);
            }

            if (!ModelState.IsValid)
                return await Failed();

            try
            {
                var entityResult = await BaseCRUDService.MapAddModelToEntity(model);

                Result<TDTO> result = null;

                if (!entityResult.IsSuccess)
                {
                    var entity = Mapper.Map<TEntity>(model);
                    result = await BaseCRUDService.Add(entity);
                }
                else
                {
                    var entity = entityResult.Item;
                    result = await BaseCRUDService.Add(entity);
                }

                if (!result.IsSuccess)
                {
                    Toast.AddErrorToastMessage(result.GetErrorMessages().FirstOrDefault());
                    return await Failed();
                }

                Toast.AddSuccessToastMessage(Message_add_success);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Toast.AddErrorToastMessage(Message_add_error);
            }


            return await Failed();
        }

        #endregion

        #region Edit

        public virtual async Task<IActionResult> Edit(TId id, bool fullPage = false)
        {
            if (fullPage)
                BreadcrumbItem(null, new { id, fullPage });

            var result = await BaseCRUDService.GetEntityById(id);

            if (!result.IsSuccess)
                return fullPage ? Error() : _Error();

            var entity = result.Item;

            var model = Mapper.Map<TUpdateModel>(entity);

            var prepareForUpdateResult = await BaseCRUDService.PrepareForUpdate(model);

            if (!prepareForUpdateResult.IsSuccess)
                return _Error();

            model = prepareForUpdateResult.Item;

            return View(model);
        }

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> Edit(TUpdateModel model)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

            async Task<IActionResult> Failed()
            {
                var result = await BaseCRUDService.PrepareForUpdate(model);

                if (!result.IsSuccess)
                    return Error();

                return View(result.Item);
            }

            if (!ModelState.IsValid)
                return await Failed();

            try
            {
                var entityResult = await BaseCRUDService.MapUpdateModelToEntity(model);

                Result<TDTO> result = null;

                if (!entityResult.IsSuccess)
                {
                    var getEntityByIdResult = await BaseCRUDService.GetEntityByIdWithoutIncludes(model.Id);

                    if (!getEntityByIdResult.IsSuccess)
                        return BadRequest(getEntityByIdResult);

                    var entity = getEntityByIdResult.Item;
                    Mapper.Map(model, entity);
                    result = await BaseCRUDService.Update(entity);
                }
                else
                {
                    var entity = entityResult.Item;
                    result = await BaseCRUDService.Update(entity);
                }

                if (!result.IsSuccess)
                {
                    Toast.AddErrorToastMessage(result.GetErrorMessages().FirstOrDefault());
                    return await Failed();
                }

                Toast.AddSuccessToastMessage(Message_edit_success);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Toast.AddErrorToastMessage(Message_edit_error);
            }

            return await Failed();
        }

        #endregion

        #region Delete

        [HttpDelete, Transaction]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            var result = await BaseCRUDService.Delete(id);

            if (!result.IsSuccess)
                return BadRequest();

            return Ok();
        }

        #endregion
    }
}
