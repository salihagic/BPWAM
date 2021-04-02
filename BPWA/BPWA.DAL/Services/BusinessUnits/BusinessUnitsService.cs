using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class BusinessUnitsService : BaseCRUDService<BusinessUnit, BusinessUnitSearchModel, BusinessUnitDTO>, IBusinessUnitsService
    {
        private CurrentUser _currentUser;

        public BusinessUnitsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            CurrentUser currentUser
            ) : base(databaseContext, mapper) 
        {
            _currentUser = currentUser;
        }

        public override IQueryable<BusinessUnit> BuildQueryConditions(IQueryable<BusinessUnit> query, BusinessUnitSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.Name.ToLower().StartsWith(searchModel.Name.ToLower()))
                       .WhereIf(_currentUser.CurrentCompanyId().HasValue, x => x.CompanyId == _currentUser.CurrentCompanyId())
                       .WhereIf(searchModel.CompanyId.HasValue, x => x.CompanyId == searchModel.CompanyId);
        }
    }
}
