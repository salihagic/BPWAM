using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface ICompaniesService : IBaseCRUDService<Company, CompanySearchModel, CompanyDTO>
    {
        Task<bool> Exists(int companyId);
        Task<List<Company>> GetSubcompanies(int companyId);
    }
}
