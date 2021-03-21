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
    [Authorize(Policy = AppClaims.Authorization.Administration.ToggleCompany)]
    [Area(Areas.Administration)]
    public class CurrentCompanyController : BaseController
    {
        private IUsersWebService _usersWebService;
        private IToastNotification _toast;

        public CurrentCompanyController(
            IUsersWebService usersWebService,
            IToastNotification toast
            )
        {
            _usersWebService = usersWebService;
            _toast = toast;
        }

        public async Task<IActionResult> Toggle() => View();

        [HttpPost]
        public async Task<IActionResult> Toggle(ToggleCurrentCompanyModel model, string returnUrl = "")
        {
            var result = await _usersWebService.ToggleCurrentCompany(model);

            if (result.IsSuccess)
                _toast.AddSuccessToastMessage(Translations.Successfully_changed_current_company);
            else
                _toast.AddErrorToastMessage(Translations.There_was_an_error_while_trying_to_change_current_company);

            return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Home", new { Area = Areas.Administration });
        }
    }
}
