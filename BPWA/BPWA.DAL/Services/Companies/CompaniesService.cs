using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class CompaniesService : BaseCRUDService<Company, CompanySearchModel, CompanyDTO>, ICompaniesService
    {
        protected ICurrentUser CurrentUser { get; }

        public CompaniesService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentUser currentUser
            ) : base(databaseContext, mapper)
        {
            CurrentUser = currentUser;
        }

        public override IQueryable<Company> BuildQueryConditions(IQueryable<Company> query, CompanySearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.Name.ToLower().StartsWith(searchModel.Name.ToLower()))
                       .Where(x => CurrentUser.CompanyIds().Contains(x.Id) || CurrentUser.HasGodMode());
        }
    }
}
