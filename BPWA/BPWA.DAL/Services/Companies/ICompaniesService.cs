using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ICompaniesService : IBaseCRUDService<Company, CompanySearchModel, CompanyDTO>
    {
    }
}
