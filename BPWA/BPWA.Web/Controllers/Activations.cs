using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    [Authorize]
    public class ActivationsController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            BreadcrumbItem("Activations");

            return View();
        }

        public async Task<IActionResult> Deactivated() => View();
    }
}
