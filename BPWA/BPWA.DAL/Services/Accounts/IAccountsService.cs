using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IAccountsService
    {
        Task UpdateTimezoneForCurrentUser(int timezoneUtcOffsetInMinutes);
        Task SendPasswordResetToken(string userId);
        Task DeleteExpiredGuestAccounts();
        Task SendAccountDeactivationWarningNotifications();
    }
}
