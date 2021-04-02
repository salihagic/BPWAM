using AutoMapper;
using BootstrapBreadcrumbs.Core;
using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Common.Resources;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Helpers;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    public class BaseReadController<TEntity, TSearchModel, TDTO> :
        BaseReadController<TEntity, TSearchModel, TDTO, int>
        where TEntity : IBaseEntity<int>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<int>, new()
    {
        public BaseReadController(
            IBaseReadWebService<TEntity, TSearchModel, TDTO, int> service,
            IMapper mapper,
            IToastNotification toast
            ) : base(service, mapper, toast) { }
    }

    public class BaseReadController<TEntity, TSearchModel, TDTO, TId> : 
        BaseController
        where TEntity : IBaseEntity<TId>, new()
        where TSearchModel : BaseSearchModel, new()
        where TDTO : BaseDTO<TId>, new()
    {
        #region Props

        public IBaseReadWebService<TEntity, TSearchModel, TDTO, TId> BaseReadService;
        public IMapper Mapper;
        public IToastNotification Toast;

        #endregion

        #region Constructor

        public BaseReadController(
            IBaseReadWebService<TEntity, TSearchModel, TDTO, TId> service,
            IMapper mapper,
            IToastNotification toast
            )
        {
            BaseReadService = service;
            Mapper = mapper;
            Toast = toast;
        }

        #endregion

        #region Index

        public virtual async Task<IActionResult> Index()
        {
            if (ShouldResetNavigationStack)
            {
                ResetNavigationStack();
            }

            BreadcrumbItem(CurrentBreadcrumbItem ?? TranslationsHelper.Translate(CurrentController));

            ViewBag.Title = CurrentBreadcrumbItem ?? TranslationsHelper.Translate(CurrentController);

            var result = await BaseReadService.PrepareForGet();

            if (!result.IsSuccess)
                return Error();

            return View(result.Item);
        }

        public virtual async Task<IActionResult> IndexJson(TSearchModel searchModel)
        {
            _sessionSearchModel = searchModel;
            var sessionSearchModel = _sessionSearchModel;

            sessionSearchModel.Pagination = GetPagination();

            var result = await BaseReadService.Get(sessionSearchModel);

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
            searchModel ??= new TSearchModel();

            var result = await BaseReadService.Get(searchModel);

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

            var result = await BaseReadService.GetById(id);

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

            var searchModel = Mapper.Map<TSearchModel>(_sessionSearchModel);
            var result = await BaseReadService.PrepareForGet(searchModel);

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
            var result = await BaseReadService.Get(searchModel);

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

        #region Session

        protected string __sessionSearchModelKey__ = $"__sessionSearchModelKey__{typeof(TSearchModel).Name}__";
        private TSearchModel _sessionSearchModel
        {
            get => HttpContext.Session.GetObject<TSearchModel>(__sessionSearchModelKey__) ?? new TSearchModel { IsDeleted = false };
            set => HttpContext.Session.SetObject(__sessionSearchModelKey__, value);
        }

        #endregion Session

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
