using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;

namespace BPWA.Web.Services.Services
{
    public interface IUsersWebService :
        IBaseCRUDWebService<User, UserSearchModel, UserDTO, UserAddModel, UserUpdateModel, string>,
        IUsersService
    {
    }
}
