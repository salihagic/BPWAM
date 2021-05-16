using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface ICompanyActivityStatusLogsService : 
        IBaseCRUDService<CompanyActivityStatusLog, CompanyActivityStatusLogSearchModel, CompanyActivityStatusLogDTO>
    {
        Task<bool> IsActive(int companyId);
        Task NotifyClientsForCacheUpdate(int companyId);
        Task RefreshCacheByCompanyId(int companyId);
    }
}
