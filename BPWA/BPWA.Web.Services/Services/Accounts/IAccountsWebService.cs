using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public interface IAccountsWebService : IAccountsService
    {
        Task<AccountUpdateModel> PrepareForUpdate();
        Task Update(AccountUpdateModel model);
        Task ToggleCurrentCompany(ToggleCurrentCompanyModel model);
        Task<ResetPasswordModel> PrepareForResetPassword(string userId, string resetPasswordToken);
        Task ResetPassword(ResetPasswordModel model);
        Task SignOut();
    }
}
