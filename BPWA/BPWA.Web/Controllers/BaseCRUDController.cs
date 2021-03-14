using AutoMapper;
using BootstrapBreadcrumbs.Core;
using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Common.Resources;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Helpers;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
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
            IBaseWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, int> service,
            IToastNotification toast,
            IMapper mapper
            ) : base(service, toast, mapper) { }
    }

    public class BaseCRUDController<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> : BaseController
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>, new()
        where TAddModel : class, new()
        where TUpdateModel : BaseUpdateModel<TId>, new()
    {
        #region Props

        public IBaseWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> _service;
        public IToastNotification _toast;
        public IMapper _mapper;

        #endregion

        #region Constructor

        public BaseCRUDController(
            IBaseWebService<TEntity, TSearchModel, TDTO, TAddModel, TUpdateModel, TId> service,
            IToastNotification toast,
            IMapper mapper
            )
        {
            _service = service;
            _toast = toast;
            _mapper = mapper;

            Message_add_success = Translations.Add_success;
            Message_add_error = Translations.Add_error;
            Message_edit_success = Translations.Edit_success;
            Message_edit_error = Translations.Edit_error;
            Message_delete_success = Translations.Delete_success;
            Message_delete_error = Translations.Delete_error;

            ShouldResetNavigationStack = true;
        }

        #endregion

        #region Index

        public virtual async Task<IActionResult> Index()
        {
            if (ShouldResetNavigationStack)
            {
                ResetNavigationStack();
            }

            BreadcrumbItem(TranslationsHelper.Translate(CurrentController));

            ViewBag.Title = TranslationsHelper.Translate(CurrentController);

            var result = await _service.PrepareForGet();

            if (!result.IsSuccess)
                return Error();

            return View(result.Item);
        }

        public virtual async Task<IActionResult> IndexJson(TSearchModel searchModel)
        {
            _sessionSearchModel = searchModel;
            var sessionSearchModel = _sessionSearchModel;

            sessionSearchModel.Pagination = GetPagination();

            var result = await _service.Get(sessionSearchModel);

            if (!result.IsSuccess)
                return Error();

            var items = result.Item;

            _sessionSearchModel = sessionSearchModel;

            var pagination = sessionSearchModel.Pagination;
            var orderField = pagination.OrderFields.FirstOrDefault();

            var sortDirection = "";
            var sortField = "";
            if (orderField != null)
            {
                sortDirection = orderField.Direction.ToString().ToLower();
                sortField = orderField.Field.ToLower();
            }

            return Ok(new
            {
                meta = new
                {
                    page = pagination.Page,
                    pages = pagination.TotalNumberOfPages,
                    perpage = pagination.Take,
                    total = pagination.TotalNumberOfRecords,
                    sort = sortDirection,
                    field = sortField
                },
                data = items
            });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Dropdown(TSearchModel searchModel)
        {
            var result = await _service.Get(searchModel);

            if (!result.IsSuccess)
                return BadRequest();

            var items = result.Item;

            return Ok(new
            {
                pagination = new
                {
                    more = searchModel.Pagination.HasMore,
                },
                results = items
            });
        }

        #endregion

        #region Details 

        public virtual async Task<IActionResult> Details(TId id, bool fullPage = false)
        {
            if (fullPage)
                BreadcrumbItem(null, new { id, fullPage });

            var result = await _service.GetById(id);

            if (!result.IsSuccess)
                return fullPage ? Error() : _Error();

            var model = result.Item;

            return View(model);
        }

        #endregion

        #region Report

        public virtual async Task<IActionResult> Report(ReportOptions reportOption)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentController);

            var searchModel = _mapper.Map<TSearchModel>(_sessionSearchModel);
            var result = await _service.PrepareForGet(searchModel);

            if (!result.IsSuccess)
                return Error();

            var model = result.Item;

            if (reportOption == ReportOptions.Pdf)
                return View(model);

            return Error();
        }

        public virtual async Task<IActionResult> ReportJson()
        {
            var searchModel = _sessionSearchModel;
            searchModel.Pagination ??= new Pagination();
            searchModel.Pagination.ShouldTakeAllRecords = true;
            var result = await _service.Get(searchModel);

            if (!result.IsSuccess)
                return BadRequest();

            var items = result.Item;

            var pagination = searchModel.Pagination;
            var orderField = pagination.OrderFields.FirstOrDefault();

            var sortDirection = "";
            var sortField = "";
            if (orderField != null)
            {
                sortDirection = orderField.Direction.ToString().ToLower();
                sortField = orderField.Field.ToLower();
            }

            return Ok(new
            {
                meta = new
                {
                    page = pagination.Page,
                    pages = pagination.TotalNumberOfPages,
                    perpage = pagination.Take,
                    total = pagination.TotalNumberOfRecords,
                    sort = sortDirection,
                    field = sortField
                },
                data = items
            });
        }

        #endregion

        #region Add

        public virtual async Task<IActionResult> Add(bool fullPage = false)
        {
            if (fullPage)
                BreadcrumbItem(null, new { fullPage });

            var result = await _service.PrepareForAdd();

            if (!result.IsSuccess)
                return fullPage ? Error() : _Error();

            var model = result.Item;

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(TAddModel model)
         {
            ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

            async Task<IActionResult> Failed()
            {
                var result = await _service.PrepareForAdd(model);

                if (!result.IsSuccess)
                    return Error();

                return View(result.Item);
            }

            if (!ModelState.IsValid)
                return await Failed();

            try
            {
                var entityResult = await _service.MapAddModelToEntity(model);

                Result<TDTO> result = null;

                if (!entityResult.IsSuccess)
                {
                    var entity = _mapper.Map<TEntity>(model);
                    result = await _service.Add(entity);
                }
                else
                {
                    var entity = entityResult.Item;
                    result = await _service.Add(entity);
                }

                if (!result.IsSuccess)
                {
                    _toast.AddErrorToastMessage(Message_add_error);
                    return await Failed();
                }

                _toast.AddSuccessToastMessage(Message_add_success);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _toast.AddErrorToastMessage(Message_add_error);
            }


            return await Failed();
        }

        #endregion

        #region Edit

        public virtual async Task<IActionResult> Edit(TId id, bool fullPage = false)
        {
            if (fullPage)
                BreadcrumbItem(null, new { id, fullPage });

            var result = await _service.GetEntityById(id);

            if (!result.IsSuccess)
                return fullPage ? Error() : _Error();

            var entity = result.Item;

            var model = _mapper.Map<TUpdateModel>(entity);

            var prepareForUpdateResult = await _service.PrepareForUpdate(model);

            if (!prepareForUpdateResult.IsSuccess)
                return _Error();

            model = prepareForUpdateResult.Item;

            return View(model);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Edit(TUpdateModel model)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentAction);

            async Task<IActionResult> Failed()
            {
                var result = await _service.PrepareForUpdate(model);

                if (!result.IsSuccess)
                    return Error();

                return View(result.Item);
            }

            if (!ModelState.IsValid)
                return await Failed();

            try
            {
                var entityResult = await _service.MapUpdateModelToEntity(model);

                Result<TDTO> result = null;

                if (!entityResult.IsSuccess)
                {
                    var getEntityByIdResult = await _service.GetEntityById(model.Id);

                    if (!getEntityByIdResult.IsSuccess)
                        return BadRequest(getEntityByIdResult);

                    var entity = getEntityByIdResult.Item;
                    _mapper.Map(model, entity);
                    result = await _service.Update(entity);
                }
                else
                {
                    var entity = entityResult.Item;
                    result = await _service.Update(entity);
                }

                if (!result.IsSuccess)
                {
                    _toast.AddErrorToastMessage(Message_edit_error);
                    return await Failed();
                }

                _toast.AddSuccessToastMessage(Message_edit_success);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _toast.AddErrorToastMessage(Message_edit_error);
            }

            return await Failed();
        }

        #endregion

        #region Delete

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            var result = await _service.Delete(id);

            if (!result.IsSuccess)
                return BadRequest();

            return Ok();
        }

        #endregion

        #region Messages 

        protected string Message_add_success;
        protected string Message_add_error;
        protected string Message_edit_success;
        protected string Message_edit_error;
        protected string Message_delete_success;
        protected string Message_delete_error;

        #endregion Messages

        #region Session

        protected string __sessionSearchModelKey__ = $"__sessionSearchModelKey__{typeof(TSearchModel).Name}__";
        private TSearchModel _sessionSearchModel
        {
            get => HttpContext.Session.GetObject<TSearchModel>(__sessionSearchModelKey__) ?? new TSearchModel { IsDeleted = false };
            set => HttpContext.Session.SetObject(__sessionSearchModelKey__, value);
        }

        #endregion Session

        #region Navigation

        protected bool ShouldResetNavigationStack { get; set; }

        protected string __sessionNavigationStackKey__ = $"__navigation__stack__";
        private List<BreadcrumbsItem> _sessionNavigationStack
        {
            get => HttpContext.Session.GetObject<List<BreadcrumbsItem>>(__sessionNavigationStackKey__) ?? new List<BreadcrumbsItem>();
            set => HttpContext.Session.SetObject(__sessionNavigationStackKey__, value);
        }

        protected string _defaultAction;
        protected string DefaultAction
        {
            get => _defaultAction = "Index";
            set => _defaultAction = value;
        }

        protected string _currentAction;
        protected string CurrentAction
        {
            get => _currentAction = HttpContext.Request.RouteValues["Action"].ToString();
            set => _currentAction = value;
        }

        protected string _currentController;
        protected string CurrentController
        {
            get => _currentController = HttpContext.Request.RouteValues["Controller"].ToString();
            set => _currentController = value;
        }

        protected void ResetNavigationStack()
        {
            var stack = _sessionNavigationStack;
            stack.Clear();
            _sessionNavigationStack = stack;
        }

        protected void BreadcrumbItem(string title = null, object routeValues = null)
        {
            var currentNavigationItem = new BreadcrumbsItem
            {
                Title = string.IsNullOrEmpty(title) ? TranslationsHelper.Translate(CurrentAction) : title,
                Controller = CurrentController,
                Action = CurrentAction,
                RouteValues = routeValues
            };

            if (_sessionNavigationStack.Count > 0)
            {
                var last = _sessionNavigationStack.Last();

                if (last.Controller == currentNavigationItem.Controller && last.Action == currentNavigationItem.Action)
                {
                    var stack1 = _sessionNavigationStack;
                    stack1.RemoveAt(stack1.Count - 1);
                    _sessionNavigationStack = stack1;
                }
                else
                {
                    this.SetBreadcrumbPrefixItems(_sessionNavigationStack);
                }
            }

            this.SetBreadcrumbAction(currentNavigationItem);

            ViewBag.Title = currentNavigationItem.Title;

            var stack = _sessionNavigationStack;
            stack.Add(currentNavigationItem);
            _sessionNavigationStack = stack;
        }

        protected void BreadcrumbPrefixItem(string title = null, string action = null, string controller = null, object routeValues = null)
        {
            this.SetBreadcrumbPrefixItems(new List<BreadcrumbsItem>
            {
                new BreadcrumbsItem
                {
                    Title = string.IsNullOrEmpty(title) ? TranslationsHelper.Translate(CurrentController) : title,
                    Controller = string.IsNullOrEmpty(controller) ? CurrentController : controller,
                    Action = string.IsNullOrEmpty(action) ? DefaultAction : action,
                    RouteValues = routeValues
                }
            });
        }

        #endregion Navigation

        #region Helpers

        public Pagination GetPagination()
        {
            var query = HttpContext.Request.Form;

            int.TryParse(query["pagination[page]"].ToString(), out var page);
            int.TryParse(query["pagination[perpage]"].ToString(), out var pageSize);
            var sortField = query["sort[field]"].ToString();
            var sortDirection = query["sort[sort]"].ToString();

            if (pageSize == 0)
                pageSize = 10;

            var pagination = new Pagination
            {
                Page = page,
                Skip = (page - 1) * pageSize,
                Take = pageSize,
                ShouldTakeAllRecords = ((page - 1) * pageSize) < 0
            };

            if (!string.IsNullOrEmpty(sortField))
            {
                pagination.OrderFields = new List<OrderField>
                {
                    new OrderField
                    {
                        Field = sortField,
                        Direction = sortDirection.ToLower() == "desc" ? SortDirection.DESC : SortDirection.ASC
                    }
                };
            }

            return pagination;
        }

        #endregion
    }
}
