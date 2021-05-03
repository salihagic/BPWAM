﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BPWA.Common.Extensions;
using BPWA.Common.Resources;
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
    public class BusinessUnitsWebService : BusinessUnitsService, IBusinessUnitsWebService
    {
        private ICurrentUser _currentUser;

        public BusinessUnitsWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentUser currentUser
            ) : base(databaseContext, mapper, currentUser)
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

        public override async Task<BusinessUnitDTO> Add(BusinessUnit entity)
        {
            var currentCompany = _currentUser.CurrentCompanyId();

            if (!currentCompany.HasValue)
                throw new Exception(Translations.No_company_is_selected);

            entity.CompanyId = currentCompany.GetValueOrDefault();

            return await base.Add(entity);
        }

        public override Task<BusinessUnitDTO> Update(BusinessUnit entity)
        {
            entity.CompanyId = _currentUser.CurrentCompanyId() ?? entity.CompanyId;

            return base.Update(entity);
        }

        public async Task<List<BusinessUnitDTO>> GetForCurrentUser()
        {
            try
            {
                var businessUnits = DatabaseContext.BusinessUnits
                    .WhereIf(!_currentUser.HasGodMode(), x => 
                    x.BusinessUnitUsers.Any(y => y.UserId == _currentUser.Id()) ||
                    x.Company.CompanyUsers.Any(y => y.UserId == _currentUser.Id())
                    );

                var businessUnitDTOs = Mapper.Map<List<BusinessUnitDTO>>(businessUnits);

                return businessUnitDTOs;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load business units");
            }
        }
    }
}
