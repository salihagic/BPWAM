using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IConfigurationService : IBaseCRUDService<Configuration, ConfigurationSearchModel, ConfigurationDTO>
    {
        Task<Configuration> FirstOrDefault();
    }
}
