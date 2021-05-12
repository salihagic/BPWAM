using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IAccountsService 
    {
        Task<UserDTO> SignIn(string userName, string password);
        Task UpdateTimezoneForCurrentUser(int timezoneUtcOffsetInMinutes);
        Task SendPasswordResetToken(string userId);
    }
}
