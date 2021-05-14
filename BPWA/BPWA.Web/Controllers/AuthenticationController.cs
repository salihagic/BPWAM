using BPWA.Common.Exceptions;
using BPWA.Common.Resources;
using BPWA.Web.Helpers;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAccountsWebService _accountsWebService;
        private readonly IToastNotification _toast;

        public AuthenticationController(
            IAccountsWebService accountsWebService,
            IToastNotification toast
            )
        {
            _accountsWebService = accountsWebService;
            _toast = toast;
        }

        public IActionResult Login() => View(new LoginModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "")
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _accountsWebService.SignIn(model.UserName, model.Password);

                return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Dashboard");
            }
            catch (ValidationException e)
            {
                _toast.AddErrorToastMessage(e.Message);
                ModelState.AddErrors(e.Messages);
            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage(e.Message);
                ModelState.AddError(Translations.Invalid_username_or_password);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountsWebService.SignOut();

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();
    }
}
