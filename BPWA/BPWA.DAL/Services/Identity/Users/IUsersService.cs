using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IUsersService : 
        IBaseCRUDService<User, UserSearchModel, UserDTO, string>
    {
        Task<User> AddEntity(User entity, string password);
    }
}
