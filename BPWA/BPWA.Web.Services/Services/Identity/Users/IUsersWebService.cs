using BPWA.DAL.Services;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public interface IUsersWebService : IUsersService
    {
        Task SignOut();
    }
}
