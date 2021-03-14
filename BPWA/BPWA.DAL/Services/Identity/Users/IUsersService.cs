using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IUsersService
    {
        Task<User> AddEntity(User entity, string password);
        Task<UserDTO> AddToRole(User entity, string roleName);
        Task<User> GetEntityByUserNameOrEmail(string userName);
        Task<UserDTO> SignIn(string userName, string password);
        Task UpdateTimezoneForLoggedUser(int timezoneUtcOffsetInMinutes);
    }
}
