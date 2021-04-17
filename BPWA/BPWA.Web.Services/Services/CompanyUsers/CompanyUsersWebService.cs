using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BPWA.Web.Services.Services
{
    public class CompanyUsersWebService : CompanyUsersService, ICompanyUsersWebService
    {
        private CurrentUser _currentUser;

        public CompanyUsersWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            CurrentUser currentUser
            ) : base(databaseContext, mapper)
        {
            _currentUser = currentUser;
        }

        public override IQueryable<CompanyUser> BuildIncludes(IQueryable<CompanyUser> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.User)
                       .ThenInclude(x => x.City);
        }

        //public override IQueryable<CompanyUser> BuildIncludesById(int id, IQueryable<CompanyUser> query)
        //{
        //    return base.BuildIncludesById(id, query)
        //               .Include(x => x.User.City)
        //               .Include(x => x.CompanyUserRoles)
        //               .Include(x => x.BusinessUnitUsers.Where(y => !CurrentUser.CurrentBusinessUnitId().HasValue || y.BusinessUnitId == CurrentUser.CurrentBusinessUnitId()))
        //               .ThenInclude(x => x.BusinessUnit)
        //               .Include(x => x.BusinessUnitUsers.Where(y => !CurrentUser.CurrentBusinessUnitId().HasValue || y.BusinessUnitId == CurrentUser.CurrentBusinessUnitId()))
        //               .ThenInclude(x => x.BusinessUnitUserRoles)
        //               .Include(x => x.UserRoles)
        //               .ThenInclude(x => x.Role);
        //}

        public override IQueryable<CompanyUser> BuildQueryConditions(IQueryable<CompanyUser> Query, CompanyUserSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(Query, searchModel)
                .Where(x => x.CompanyId == _currentUser.CurrentCompanyId());
        }
    }
}
