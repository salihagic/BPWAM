using BPWA.Common.Resources;
using BPWA.Common.Security;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using BPWA.Common.Extensions;

namespace BPWA.Controllers
{
    public class AppClaimsController : BaseController
    {
        private IAppClaimsService _appClaimsService;

        public AppClaimsController(IAppClaimsService appClaimsService)
        {
            _appClaimsService = appClaimsService;
        }

        [HttpPost]
        public virtual IActionResult Dropdown(AppClaimsSearchModel searchModel)
        {
            var itemsResult = _appClaimsService.Get(searchModel);

            if (!itemsResult.IsSuccess)
                return BadRequest();

            return Ok(new
            {
                pagination = new
                {
                    more = AppClaimsHelper.Authorization.All.Count() > (searchModel.Pagination.Skip.GetValueOrDefault() * searchModel.Pagination.Take.GetValueOrDefault() + searchModel.Pagination.Take.GetValueOrDefault()),
                },
                results = itemsResult.Item.Select(x => new SelectListItem
                {
                    Value = x,
                    Text = TranslationsHelper.Translate(x)
                    //Text = Translations.ResourceManager.GetString(x, Translations.Culture)
                })
            });
        }
    }
}
