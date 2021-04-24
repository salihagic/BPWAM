using BPWA.Common.Resources;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Administration.LogsRead)]
    public class LogsController : BaseController
    {
        private readonly ILogsService _logsService;

        public LogsController(ILogsService logsService)
        {
            _logsService = logsService;
        }

        #region Index

        public virtual async Task<IActionResult> Index()
        {
            ResetNavigationStack();

            BreadcrumbItem(Translations.Logs);

            ViewBag.Title = Translations.Logs;

            var searchModel = new LogSearchModel();

            return View(searchModel);
        }

        public virtual async Task<IActionResult> IndexJson(LogSearchModel searchModel)
        {
            searchModel.Pagination = GetPagination();

            var result = await _logsService.Get(searchModel);

            if (!result.IsSuccess)
                return Error();

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

        #region Details 

        public virtual async Task<IActionResult> Details(int id)
        {
            var result = await _logsService.GetById(id);

            if (!result.IsSuccess)
                return _Error();

            return View(result.Item);
        }

        #endregion
    }
}
