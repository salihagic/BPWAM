using BPWA.Common.Resources;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Web.Helpers.Routing;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Threading.Tasks;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Company.ToggleBusinessUnit)]
    [Area(Areas.Administration)]
    public class CurrentBusinessUnitController : BaseController
    {
        private IUsersWebService _usersWebService;
        private IToastNotification _toast;

        public CurrentBusinessUnitController(
            IUsersWebService usersWebService,
            IToastNotification toast
            )
        {
            _usersWebService = usersWebService;
            _toast = toast;
        }

        public async Task<IActionResult> Toggle() => View();

        [HttpPost]
        public async Task<IActionResult> Toggle(ToggleCurrentBusinessUnitModel model, string returnUrl = "")
        {
            var result = await _usersWebService.ToggleCurrentBusinessUnit(model);

            if (result.IsSuccess)
                _toast.AddSuccessToastMessage(Translations.Successfully_changed_current_business_unit);
            else
                _toast.AddErrorToastMessage(Translations.There_was_an_error_while_trying_to_change_current_business_unit);

            return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Home", new { Area = Areas.Administration });
        }
    }
}
