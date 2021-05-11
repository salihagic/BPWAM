using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class AccountTypesService : BaseCRUDService<AccountType, AccountTypeSearchModel, AccountTypeDTO>, IAccountTypesService
    {
        public AccountTypesService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }

        public override IQueryable<AccountType> BuildQueryConditions(IQueryable<AccountType> query, AccountTypeSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(searchModel.SystemAccountType.HasValue, x => x.SystemAccountType == searchModel.SystemAccountType);
        }
    }
}
