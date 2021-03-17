using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPWA.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        public IActionResult Index() => View();
    }
}
