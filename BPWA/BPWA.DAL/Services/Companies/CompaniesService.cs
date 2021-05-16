using AutoMapper;
using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class CompaniesService : 
        BaseCRUDService<Company, CompanySearchModel, CompanyDTO>, 
        ICompaniesService
    {
        private ICompanyActivityStatusLogsService _companyActivityStatusLogsService;

        public CompaniesService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICompanyActivityStatusLogsService companyActivityStatusLogsService
            ) : base(databaseContext, mapper)
        {
            _companyActivityStatusLogsService = companyActivityStatusLogsService;
        }

        public override IQueryable<Company> BuildQueryConditions(IQueryable<Company> query, CompanySearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(!string.IsNullOrEmpty(searchModel?.SearchTerm), x => x.Name.ToLower().StartsWith(searchModel.SearchTerm.ToLower()))
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.Name.ToLower().StartsWith(searchModel.Name.ToLower()));
        }

        public override async Task<Company> AddEntity(Company entity)
        {
            await base.AddEntity(entity);

            await _companyActivityStatusLogsService.Add(new CompanyActivityStatusLog
            {
                CompanyId = entity.Id,
                ActivityStatus = ActivityStatus.Active
            });

            return entity;
        }

        public async Task<bool> Exists(int companyId)
        {
            return await DatabaseContext.Companies
                .IgnoreQueryFilters()
                .AnyAsync(x => !x.IsDeleted && x.Id == companyId);
        }
    }
}
