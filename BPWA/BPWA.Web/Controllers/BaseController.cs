using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPWA.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public virtual IActionResult Error() => RedirectToAction("Error", "Home");
        public virtual IActionResult _Error() => RedirectToAction("_Error", "Home");
    }
}
