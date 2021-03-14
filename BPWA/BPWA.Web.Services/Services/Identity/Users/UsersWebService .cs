using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class UsersWebService : UsersService, IUsersWebService
    {
        public UsersWebService(
            IMapper mapper,
            DatabaseContext databaseContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILoggedUserService loggedUserService
            ) : base(
                databaseContext,
                mapper,
                userManager,
                signInManager,
                loggedUserService
                )
        {
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
