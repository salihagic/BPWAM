using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Common.Resources;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TFM.DAL.Models;

namespace BPWA.DAL.Services
{
    public class BusinessUnitsWebService : BusinessUnitsService, IBusinessUnitsWebService
    {
        private CurrentUser _currentUser;

        public BusinessUnitsWebService(
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
                       .WhereIf(_currentUser.CurrentCompanyId().HasValue, x => x.CompanyId == _currentUser.CurrentCompanyId());                       
        }

        public override IQueryable<BusinessUnit> BuildIncludes(IQueryable<BusinessUnit> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.Company);
        }

        public override IQueryable<BusinessUnit> BuildIncludesById(int id, IQueryable<BusinessUnit> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.Company);
        }

        public override async Task<Result<BusinessUnitDTO>> Add(BusinessUnit entity)
        {
            var currentCompany = _currentUser.CurrentCompanyId();

            if (!currentCompany.HasValue)
                return Result.Failed<BusinessUnitDTO>(Translations.No_company_is_selected);

            entity.CompanyId = currentCompany.GetValueOrDefault();

            return await base.Add(entity);
        }

        public override Task<Result<BusinessUnitDTO>> Update(BusinessUnit entity)
        {
            entity.CompanyId = _currentUser.CurrentCompanyId() ?? entity.CompanyId;

            return base.Update(entity);
        }
    }
}
