﻿using BPWA.Common.Resources;
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
        private readonly ICompaniesWebService _companiesWebService;
        private readonly IBusinessUnitsWebService _businessUnitsWebService;
        private IToastNotification _toast;

        public AccountController(
            IUsersWebService usersWebService,
            ICompaniesWebService companiesWebService,
            IBusinessUnitsWebService businessUnitsWebService,
            IToastNotification toast
            )
        {
            _usersWebService = usersWebService;
            _companiesWebService = companiesWebService;
            _businessUnitsWebService = businessUnitsWebService;
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
        public virtual async Task<IActionResult> CurrentUserCompaniesDropdown()
        {
            var result = await _companiesWebService.GetForCurrentUser();

            if (!result.IsSuccess)
                return BadRequest();

            return Ok(new
            {
                pagination = new
                {
                    more = false,
                },
                results = result.Item
            });
        }

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
        public virtual async Task<IActionResult> CurrentUserBusinessUnitsDropdown()
        {
            var result = await _businessUnitsWebService.GetForCurrentUser();

            if (!result.IsSuccess)
                return BadRequest();

            return Ok(new
            {
                pagination = new
                {
                    more = false,
                },
                results = result.Item
            });
        }


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
