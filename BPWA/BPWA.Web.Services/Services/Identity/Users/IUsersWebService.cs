using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.Web.Services.Services
{
    public interface IUsersWebService : IUsersService
    {
        Task SignOut();
        Task<Result> ToggleCurrentCompany(ToggleCurrentCompanyModel model);
        Task<Result> ToggleCurrentBusinessUnit(ToggleCurrentBusinessUnitModel model);
    }
}
