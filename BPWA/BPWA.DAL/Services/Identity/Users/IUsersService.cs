using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IUsersService : 
        IBaseCRUDService<User, UserSearchModel, UserDTO, string>
    {
        Task<Result<User>> AddEntity(User entity, string password);
        Task<Result<UserDTO>> AddToRole(User entity, string roleName);
        Task<Result<User>> GetEntityByUserNameOrEmail(string userNameOrEmail);
        Task<Result<UserDTO>> SignIn(string userName, string password);
        Task<Result> UpdateTimezoneForCurrentUser(int timezoneUtcOffsetInMinutes);
        Task<Result> SendPasswordResetToken(string userId);
    }
}
