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
        private ICurrentUser _currentUser;

        public CompanyUsersWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentUser currentUser
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

        public override IQueryable<CompanyUser> BuildQueryConditions(IQueryable<CompanyUser> Query, CompanyUserSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(Query, searchModel)
                .Where(x => x.CompanyId == _currentUser.CurrentCompanyId());
        }
    }
}
