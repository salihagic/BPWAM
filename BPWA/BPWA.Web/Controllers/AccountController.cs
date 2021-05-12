using BPWA.Common.Resources;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Helpers.Filters;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountsWebService _accountsWebService;
        private readonly ICompaniesWebService _companiesWebService;
        private IToastNotification _toast;
        private ICurrentUserBaseCompany _currentUserBaseCompany;

        public AccountController(
            IAccountsWebService accountsWebService,
            ICompaniesWebService companiesWebService,
            IToastNotification toast,
            ICurrentUserBaseCompany currentUserBaseCompany
            )
        {
            _accountsWebService = accountsWebService;
            _companiesWebService = companiesWebService;
            _toast = toast;
            _currentUserBaseCompany = currentUserBaseCompany;
        }

        [HttpPost]
        public async Task UpdateTimezone(int timezoneUtcOffsetInMinutes)
        {
            await _accountsWebService.UpdateTimezoneForCurrentUser(timezoneUtcOffsetInMinutes);
        }

        #region Update account

        public async Task<IActionResult> Edit()
        {
            BreadcrumbItem(Translations.My_profile);

            var result = await _accountsWebService.PrepareForUpdate();

            return View(result);
        }

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> Edit(AccountUpdateModel model)
        {
            ViewBag.Title = Translations.My_profile;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _accountsWebService.Update(model);

                _toast.AddSuccessToastMessage("Successfully edited my profile");
                return RedirectToAction(nameof(Edit));
            }
            catch (Exception ex)
            {
                _toast.AddErrorToastMessage("Failed to edit my profile");
            }


            return View(model);
        }

        #endregion 

        #region Toggle current company

        public async Task<IActionResult> ToggleCurrentCompany() => View();

        [HttpPost]
        public virtual async Task<IActionResult> CurrentUserCompaniesDropdown(CompanySearchModel searchModel)
        {
            var result = await _companiesWebService.GetForToggle(searchModel);
            var dropdownItems = result.Select(x => new DropdownItem
            {
                Id = x.Id,
                Text = x.Name,
            }).ToList();

            if (!_currentUserBaseCompany.Id().HasValue)
            {
                dropdownItems.Insert(0, new DropdownItem
                {
                    Id = 0,
                    Text = Translations.All_companies
                });
            }

            return Ok(new
            {
                pagination = new
                {
                    more = false,
                },
                results = dropdownItems
            });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleCurrentCompany(ToggleCurrentCompanyModel model, string returnUrl = "")
        {
            try
            {
                await _accountsWebService.ToggleCurrentCompany(model);
                _toast.AddSuccessToastMessage(Translations.Successfully_changed_current_company);

            }
            catch (Exception)
            {
                _toast.AddErrorToastMessage(Translations.There_was_an_error_while_trying_to_change_current_company);
            }

            return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Dashboard");
        }

        #endregion Toggle current company

        #region Reset password

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            var result = await _accountsWebService.PrepareForResetPassword(userId, token);

            var model = result;

            return View(model);
        }

        [HttpPost, Transaction, AllowAnonymous]
        public virtual async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            async Task<IActionResult> Failed()
            {
                return View(model);
            }

            if (!ModelState.IsValid)
                return await Failed();

            try
            {
                await _accountsWebService.ResetPassword(model);

                _toast.AddSuccessToastMessage(Translations.Successfully_changed_password);
                return RedirectToAction("Login", "Authentication");
            }
            catch (Exception ex)
            {
                _toast.AddErrorToastMessage(Translations.Failed_to_change_password);
            }


            return await Failed();
        }

        #endregion
    }
}
