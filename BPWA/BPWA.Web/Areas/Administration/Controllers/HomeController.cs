using BPWA.Controllers;
using BPWA.Web.Helpers.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPWA.Administration.Controllers
{
    [Authorize]
    [Area(Areas.Administration)]
    public class HomeController : BaseController
    {
        public IActionResult Index() => View();
    }
}
