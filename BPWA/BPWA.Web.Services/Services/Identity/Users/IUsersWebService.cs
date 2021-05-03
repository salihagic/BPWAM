using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public interface IUsersWebService :
        IBaseCRUDWebService<User, UserSearchModel, UserDTO, UserAddModel, UserUpdateModel, string>,
        IUsersService
    {
        Task<AccountUpdateModel> PrepareForUpdateAccount();
        Task UpdateAccount(AccountUpdateModel model);
        Task ToggleCurrentCompany(ToggleCurrentCompanyModel model);
        Task ToggleCurrentBusinessUnit(ToggleCurrentBusinessUnitModel model);
        Task<ResetPasswordModel> PrepareForResetPassword(string userId, string resetPasswordToken);
        Task ResetPassword(ResetPasswordModel model);
        Task SignOut();
    }
}
