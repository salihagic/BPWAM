using AutoMapper;
using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
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
        private ICurrentBaseCompany _currentBaseCompany;
        private IAccountsWebService _accountsWebService;

        public CompaniesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentBaseCompany currentBaseCompany,
            IAccountsWebService accountsWebService,
            ICompanyActivityStatusLogsService companyActivityStatusLogsService
            ) : base(databaseContext, mapper, companyActivityStatusLogsService)
        {
            _currentBaseCompany = currentBaseCompany;
            _accountsWebService = accountsWebService;
        }

        public async Task<List<CompanyDTO>> GetForToggle(CompanySearchModel searchModel)
        {
            var companies = await DatabaseContext.Companies
                .IgnoreQueryFilters()
                .Where(x => !x.IsDeleted)
                .WhereIf(searchModel.SearchTerm.IsNotEmpty(), x => x.Name.ToLower().StartsWith(searchModel.SearchTerm.ToLower()))
                .Where(x =>
                //All
                ((_currentBaseCompany.Id() == null && x.AccountType == AccountType.Regular) ||
                //Level 0 company
                (x.Id == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular)) ||
                //Level 1 company
                (x.CompanyId == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular)) ||
                //Level 2 company
                (x.Company.CompanyId == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular)) ||
                //Level 3 company
                (x.Company.Company.CompanyId == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular)) ||
                //Level 4 company
                (x.Company.Company.Company.CompanyId == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular))
                //...
                )).ToListAsync();

            var companyDTOs = Mapper.Map<List<CompanyDTO>>(companies);

            return companyDTOs;
        }

        public override async Task<Company> AddEntity(Company entity)
        {
            entity.AccountType = _currentBaseCompany.AccountType() ?? AccountType.Regular;
            var result = await base.AddEntity(entity);

            await _accountsWebService.RefreshSignIn();

            return result;
        }

        public override async Task<Company> UpdateEntity(Company entity)
        {
            var result = await base.UpdateEntity(entity);

            await _accountsWebService.RefreshSignIn();

            return result;
        }

        public override async Task Delete(Company entity)
        {
            await base.Delete(entity);

            await _accountsWebService.RefreshSignIn();
        }
    }
}
