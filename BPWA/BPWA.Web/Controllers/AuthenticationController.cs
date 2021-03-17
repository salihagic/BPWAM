using BPWA.Common.Exceptions;
using BPWA.Common.Resources;
using BPWA.Web.Helpers;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : BaseController
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
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _usersWebService.SignIn(model.UserName, model.Password);

                return RedirectToAction("Index", "Dashboard");
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
