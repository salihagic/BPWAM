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
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    public class BaseCRUDController<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel> :
        BaseCRUDController<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, int>
        where TEntity : class, IBaseEntity<int>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<int>, new()
        where TAddModel : class, new()
        where TUpdateModel : class, IBaseUpdateModel, new()
    {
        public BaseCRUDController(
            IBaseCRUDWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, int> service,
            IMapper mapper,
            IToastNotification toast
            ) : base(service, mapper, toast) { }
    }

    public class BaseCRUDController<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> :
        BaseReadController<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>, new()
        where TAddModel : class, new()
        where TUpdateModel : class, IBaseUpdateModel<TId>, new()
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

        [HttpPost]
        public virtual async Task<IActionResult> Add(TAddModel model, bool fullPage = false)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                try
                {
                    await BaseCRUDService.Add(model);
                }
                catch (NotImplementedException)
                {
                    var entity = Mapper.Map<TEntity>(model);
                    await BaseCRUDService.Add(entity);
                }

                Toast.AddSuccessToastMessage(Message_add_success);
                return fullPage ? RedirectToAction("Index") : Json(new { success = true });
            }
            catch (Exception)
            {
                Toast.AddErrorToastMessage(Message_add_error);
            }

            return View(model);
        }

        #endregion

        #region Edit

        public virtual async Task<IActionResult> Edit(TId id, bool fullPage = false)
        {
            try
            {
                if (fullPage)
                    BreadcrumbItem(null, new { id, fullPage });

                TUpdateModel model = null;

                try
                {
                    model = await BaseCRUDService.PrepareForUpdate(id);
                }
                catch (NotImplementedException)
                {
                    var entity = await BaseCRUDService.GetEntityById(id, false, true);
                    model = Mapper.Map<TUpdateModel>(entity);
                }

                return View(model);
            }
            catch (Exception)
            {
                return fullPage ? Error() : _Error();
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Edit(TUpdateModel model, bool fullPage = false)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                try
                {
                    await BaseCRUDService.Update(model);
                }
                catch (NotImplementedException)
                {
                    var entity = await BaseCRUDService.GetEntityById(model.Id, false, false);
                    Mapper.Map(model, entity);
                    await BaseCRUDService.Update(entity);
                }

                Toast.AddSuccessToastMessage(Message_edit_success);
                return fullPage ? RedirectToAction("Index") : Json(new { success = true });
            }
            catch (Exception exception)
            {
                Toast.AddErrorToastMessage(Message_edit_error);
            }

            return View(model);
        }

        #endregion

        #region Delete

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            try
            {
                await BaseCRUDService.Delete(id);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        #endregion
    }
}
