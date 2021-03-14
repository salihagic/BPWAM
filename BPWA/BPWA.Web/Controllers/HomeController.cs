﻿using BPWA.Common.Security;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BPWA.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILoggedUserService _loggedUserService;

        public HomeController(ILoggedUserService loggedUserService)
        {

            _loggedUserService = loggedUserService;
        }

        public IActionResult Index() => View();

        public IActionResult Error() => View();

        public IActionResult _Error() => View();

        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Path = "/",
                    Expires = DateTime.Now.AddYears(1),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    Domain = HttpContext.Request.Host.Host
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
