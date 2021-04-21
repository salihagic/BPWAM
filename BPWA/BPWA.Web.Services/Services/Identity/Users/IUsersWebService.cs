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
        Task<Result<AccountUpdateModel>> PrepareForUpdateAccount();
        Task<Result> UpdateAccount(AccountUpdateModel model);
        Task<Result> ToggleCurrentCompany(ToggleCurrentCompanyModel model);
        Task<Result> ToggleCurrentBusinessUnit(ToggleCurrentBusinessUnitModel model);
        Task<Result<ResetPasswordModel>> PrepareForResetPassword(string userId, string resetPasswordToken);
        Task<Result> ResetPassword(ResetPasswordModel model);
        Task SignOut();
    }
}
