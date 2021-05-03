using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IUsersService : 
        IBaseCRUDService<User, UserSearchModel, UserDTO, string>
    {
        Task<User> AddEntity(User entity, string password);
        Task<UserDTO> AddToRole(User entity, string roleName);
        Task<User> GetEntityByUserNameOrEmail(string userNameOrEmail);
        Task<UserDTO> SignIn(string userName, string password);
        Task UpdateTimezoneForCurrentUser(int timezoneUtcOffsetInMinutes);
        Task SendPasswordResetToken(string userId);
    }
}
