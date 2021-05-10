using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public interface IConfigurationWebService : IConfigurationService
    {
        Task<ConfigurationUpdateModel> PrepareForUpdate();
        Task Update(ConfigurationUpdateModel model);
    }
}
