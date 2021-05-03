using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class CompaniesWebService : CompaniesService, ICompaniesWebService
    {
        public CompaniesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentUser currentUser
            ) : base(databaseContext, mapper, currentUser)
        {
        }

        public async Task<List<CompanyDTO>> GetForCurrentUser()
        {
            var companies = DatabaseContext.Companies
                .WhereIf(!CurrentUser.HasGodMode(), x => x.CompanyUsers.Any(y => y.UserId == CurrentUser.Id()));

            var companyDTOs = Mapper.Map<List<CompanyDTO>>(companies);

            return companyDTOs;
        }
    }
}
