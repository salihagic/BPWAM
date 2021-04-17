using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class CompanyUsersService : BaseCRUDService<CompanyUser, CompanyUserSearchModel, CompanyUserDTO>, ICompanyUsersService
    {
        public CompanyUsersService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }

        public override IQueryable<CompanyUser> BuildQueryConditions(IQueryable<CompanyUser> Query, CompanyUserSearchModel searchModel = null)
        {
            if (searchModel == null)
                return Query;

            return Query
                .WhereIf(searchModel.IsDeleted.HasValue, x => x.IsDeleted == searchModel.IsDeleted.Value)
                .WhereIf(!string.IsNullOrEmpty(searchModel.UserName), x => x.User.UserName.ToLower().StartsWith(searchModel.UserName.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.Email), x => x.User.Email.ToLower().StartsWith(searchModel.Email.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.FirstName), x => x.User.FirstName.ToLower().StartsWith(searchModel.FirstName.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(searchModel.LastName), x => x.User.LastName.ToLower().StartsWith(searchModel.LastName.ToLower()));
        }
    }
}
