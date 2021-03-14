using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BPWA.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUsersWebService _usersWebService;

        public AccountController(IUsersWebService usersWebService)
        {
            _usersWebService = usersWebService;
        }

        [AllowAnonymous, HttpPost]
        public async Task UpdateTimezone(int timezoneUtcOffsetInMinutes)
        {
            await _usersWebService.UpdateTimezoneForLoggedUser(timezoneUtcOffsetInMinutes);
        }
    }
}
