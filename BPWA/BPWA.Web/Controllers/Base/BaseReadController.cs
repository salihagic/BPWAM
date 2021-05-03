using AutoMapper;
using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Helpers;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    public class BaseReadController<TEntity, TSearchModel, TDTO> :
        BaseReadController<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity<int>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<int>, new()
    {
        public BaseReadController(
            IBaseReadWebService<TEntity, TSearchModel, TDTO, int> service,
            IMapper mapper,
            IToastNotification toast
            ) : base(service, mapper, toast) { }
    }

    public class BaseReadController<TEntity, TSearchModel, TDTO, TId> :
        BaseController
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : class, IBaseSearchModel, new()
        where TDTO : class, IBaseDTO<TId>, new()
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

            //TODO: Fix form reset on Index pages
            //var searchModel = _sessionSearchModel ?? (await BaseReadService.PrepareForGet()).Item;

            return View(new TSearchModel());
        }

        public virtual async Task<IActionResult> IndexJson(TSearchModel searchModel)
        {
            try
            {
                _sessionSearchModel = searchModel;
                var sessionSearchModel = _sessionSearchModel;

                sessionSearchModel.Pagination = GetPagination();

                var items = await BaseReadService.Get(sessionSearchModel);

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
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Dropdown(TSearchModel searchModel)
        {
            try
            {
                searchModel ??= new TSearchModel();

                var items = await BaseReadService.Get(searchModel);

                return Ok(new
                {
                    pagination = new
                    {
                        more = searchModel.Pagination.HasMore,
                    },
                    results = items
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion

        #region Details 

        public virtual async Task<IActionResult> Details(TId id, bool fullPage = false)
        {
            try
            {
                if (fullPage)
                    BreadcrumbItem(null, new { id, fullPage });

                var result = await BaseReadService.GetById(id);

                return View(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion

        #region Report

        public virtual async Task<IActionResult> Report(ReportOptions reportOption)
        {
            ViewBag.Title = TranslationsHelper.Translate(CurrentController);

            var model = _sessionSearchModel;

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

            var items = result;

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
            get => HttpContext.Session.GetObject<TSearchModel>(__sessionSearchModelKey__);
            set => HttpContext.Session.SetObject(__sessionSearchModelKey__, value);
        }

        #endregion Session
    }
}
