using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUsersWebService _usersWebService;
        private IToastNotification _toast;

        public AccountController(
            IUsersWebService usersWebService,
            IToastNotification toast
            )
        {
            _usersWebService = usersWebService;
            _toast = toast;
        }

        [HttpPost]
        public async Task UpdateTimezone(int timezoneUtcOffsetInMinutes)
        {
            await _usersWebService.UpdateTimezoneForCurrentUser(timezoneUtcOffsetInMinutes);
        }

        #region Toggle current company

        public async Task<IActionResult> ToggleCurrentCompany() => View();

        [HttpPost]
        public async Task<IActionResult> ToggleCurrentCompany(ToggleCurrentCompanyModel model, string returnUrl = "")
        {
            var result = await _usersWebService.ToggleCurrentCompany(model);

            if (result.IsSuccess)
                _toast.AddSuccessToastMessage(Translations.Successfully_changed_current_company);
            else
                _toast.AddErrorToastMessage(Translations.There_was_an_error_while_trying_to_change_current_company);

            return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Dashboard");
        }

        #endregion Toggle current company

        #region Toggle current business unit

        public async Task<IActionResult> ToggleCurrentBusinessUnit() => View();

        [HttpPost]
        public async Task<IActionResult> ToggleCurrentBusinessUnit(ToggleCurrentBusinessUnitModel model, string returnUrl = "")
        {
            var result = await _usersWebService.ToggleCurrentBusinessUnit(model);

            if (result.IsSuccess)
                _toast.AddSuccessToastMessage(Translations.Successfully_changed_current_business_unit);
            else
                _toast.AddErrorToastMessage(Translations.There_was_an_error_while_trying_to_change_current_business_unit);

            return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Dashboard");
        }

        #endregion Toggle current business unit
    }
}
