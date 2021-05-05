using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public class CompaniesService : BaseCRUDService<Company, CompanySearchModel, CompanyDTO>, ICompaniesService
    {
        public CompaniesService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }
    }
}
