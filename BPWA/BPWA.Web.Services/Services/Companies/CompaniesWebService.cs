using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class CompaniesWebService : CompaniesService, ICompaniesWebService
    {
        private ICurrentUserBaseCompany _currentUserBaseCompany;

        public CompaniesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentUserBaseCompany currentUserBaseCompany
            ) : base(databaseContext, mapper)
        {
            _currentUserBaseCompany = currentUserBaseCompany;
        }

        public async Task<List<CompanyDTO>> GetForToggle()
        {
            var companies = await DatabaseContext.Companies
                .IgnoreQueryFilters()
                .Where(x => !x.IsDeleted)
                .Where(x => _currentUserBaseCompany.Id() == null ||
                    //Level 0 company
                    x.Id == _currentUserBaseCompany.Id() ||
                    //Level 1 company
                    x.CompanyId == _currentUserBaseCompany.Id() ||
                    //Level 2 company
                    x.Company.CompanyId == _currentUserBaseCompany.Id() ||
                    //Level 3 company
                    x.Company.Company.CompanyId == _currentUserBaseCompany.Id() ||
                    //Level 4 company
                    x.Company.Company.Company.CompanyId == _currentUserBaseCompany.Id()
                //...
                ).ToListAsync();

            var companyDTOs = Mapper.Map<List<CompanyDTO>>(companies);

            return companyDTOs;
        }
    }
}
