using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class CompaniesWebService : CompaniesService, ICompaniesWebService
    {
        private CurrentUser _currentUser;

        public CompaniesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            CurrentUser currentUser
            ) : base(databaseContext, mapper)
        {
            _currentUser = currentUser;
        }

        public override IQueryable<Company> BuildQueryConditions(IQueryable<Company> query, CompanySearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(_currentUser.CompanyId().HasValue, x => x.Id == _currentUser.CompanyId()); ;
        }
    }
}
