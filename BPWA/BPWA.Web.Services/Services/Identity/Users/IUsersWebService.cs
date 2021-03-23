using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.Web.Services.Services
{
    public interface IUsersWebService : IUsersService
    {
        Task SignOut();
        Task<Result> ToggleCompany(ToggleCompanyModel model);
        Task<Result> ToggleBusinessUnit(ToggleBusinessUnitModel model);
    }
}
