using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class CompaniesWebService : CompaniesService, ICompaniesWebService
    {
        public CompaniesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentUser currentUser
            ) : base(databaseContext, mapper, currentUser)
        {
        }

        public async Task<List<CompanyDTO>> GetForCurrentUser()
        {
            var companies = DatabaseContext.Companies;

            var companyDTOs = Mapper.Map<List<CompanyDTO>>(companies);

            return companyDTOs;
        }
    }
}
