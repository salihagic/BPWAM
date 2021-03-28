using AutoMapper;
using AutoMapper.QueryableExtensions;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using Microsoft.EntityFrameworkCore;
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
            CurrentUser currentUser
            ) : base(databaseContext, mapper, currentUser)
        {
        }

        public async Task<Result<List<CompanyDTO>>> GetForCurrentUser()
        {
            try
            {
                var companies = DatabaseContext.Companies
                    .WhereIf(!CurrentUser.HasGodMode(), x => x.CompanyUsers.Any(y => y.UserId == CurrentUser.Id()));

                var companyDTOs = Mapper.Map<List<CompanyDTO>>(companies);

                return Result.Success(companyDTOs);
            }
            catch (Exception e)
            {
                return Result.Failed<List<CompanyDTO>>("Failed to load companies");
            }
        }
    }
}
