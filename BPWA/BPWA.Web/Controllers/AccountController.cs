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
            await _usersWebService.UpdateTimezoneForLoggedUser(timezoneUtcOffsetInMinutes);
        }

        #region Toggle company

        public async Task<IActionResult> ToggleCompany() => View();

        [HttpPost]
        public async Task<IActionResult> ToggleCompany(ToggleCompanyModel model, string returnUrl = "")
        {
            var result = await _usersWebService.ToggleCompany(model);

            if (result.IsSuccess)
                _toast.AddSuccessToastMessage(Translations.Successfully_changed_current_company);
            else
                _toast.AddErrorToastMessage(Translations.There_was_an_error_while_trying_to_change_current_company);

            return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Dashboard");
        }

        #endregion Toggle company

        #region Toggle Business unit

        public async Task<IActionResult> ToggleBusinessUnit() => View();

        [HttpPost]
        public async Task<IActionResult> ToggleBusinessUnit(ToggleBusinessUnitModel model, string returnUrl = "")
        {
            var result = await _usersWebService.ToggleBusinessUnit(model);

            if (result.IsSuccess)
                _toast.AddSuccessToastMessage(Translations.Successfully_changed_current_business_unit);
            else
                _toast.AddErrorToastMessage(Translations.There_was_an_error_while_trying_to_change_current_business_unit);

            return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Dashboard");
        }

        #endregion Toggle Business unit
    }
}
