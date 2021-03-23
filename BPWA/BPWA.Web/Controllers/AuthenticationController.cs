using BPWA.Common.Exceptions;
using BPWA.Common.Resources;
using BPWA.Web.Helpers;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUsersWebService _usersWebService;

        public AuthenticationController(
            IUsersWebService usersWebService
            )
        {
            _usersWebService = usersWebService;
        }

        public IActionResult Login() => View(new LoginModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "")
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _usersWebService.SignIn(model.UserName, model.Password);

                return !string.IsNullOrEmpty(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Dashboard");
            }
            catch (ValidationException e)
            {
                ModelState.AddErrors(e.Errors);
            }
            catch (Exception e)
            {
                ModelState.AddError(Translations.Invalid_username_or_password);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _usersWebService.SignOut();

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();
    }
}
