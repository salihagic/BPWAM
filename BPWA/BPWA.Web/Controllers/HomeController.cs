using BPWA.Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace BPWA.Controllers
{
    public class HomeController : BaseController
    {
        private ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Title = Translations.BPWA;
            _logger.LogError("Erorcina");
            return View();
        }

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
                    Domain = HttpContext.Request.Host.Host,
                    SameSite = SameSiteMode.None
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
