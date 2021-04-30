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
using BPWA.DAL.Models;

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
            IMapper mapper,
            IToastNotification toast
            ) : base(service, mapper, toast) { }
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
            IMapper mapper,
            IToastNotification toast
            ) : base(service, mapper, toast)
        {
            BaseCRUDService = service;

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

            return View(new TAddModel());
        }

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> Add(TAddModel model)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await BaseCRUDService.Add(model);

                if (!result.IsSuccess)
                {
                    Toast.AddErrorToastMessage(result.GetErrorMessages().FirstOrDefault());
                    return View(model);
                }

                Toast.AddSuccessToastMessage(Message_add_success);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Toast.AddErrorToastMessage(Message_add_error);
            }


            return View(model);
        }

        #endregion

        #region Edit

        public virtual async Task<IActionResult> Edit(TId id, bool fullPage = false)
        {
            if (fullPage)
                BreadcrumbItem(null, new { id, fullPage });

            var modelResult = await BaseCRUDService.PrepareForUpdate(id);

            if (!modelResult.IsSuccess)
                return fullPage ? Error() : _Error();

            return View(modelResult.Item);
        }

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> Edit(TUpdateModel model, bool fullPage = false)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var entityResult = await BaseCRUDService.MapUpdateModelToEntity(model);

                Result<TDTO> result = null;

                if (!entityResult.IsSuccess)
                {
                    var getEntityByIdResult = await BaseCRUDService.GetEntityById(model.Id, shouldTranslate: false, includeRelated: false);

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
                    return View(model);
                }

                Toast.AddSuccessToastMessage(Message_edit_success);
                return fullPage ? RedirectToAction("Index") : Json(new { success = true });
            }
            catch (Exception ex)
            {
                Toast.AddErrorToastMessage(Message_edit_error);
            }

            return View(model);
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
