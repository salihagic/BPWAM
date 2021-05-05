using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public interface ICompaniesWebService :
        IBaseCRUDWebService<Company, CompanySearchModel, CompanyDTO, CompanyAddModel, CompanyUpdateModel>,
        ICompaniesService
    {
        Task<List<CompanyDTO>> GetForToggle();
    }
}
