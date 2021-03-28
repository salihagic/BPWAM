using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Common.Security;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.Web.Services.Services
{
    public class DropdownHelperService : IDropdownHelperService
    {
        private CurrentUser _currentUser;

        public DropdownHelperService(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        virtual public List<SelectListItem> GetAppClaims()
        {
            var claims = new List<string>();

            if (_currentUser.HasGodMode() || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement))
            {
                claims.AddRange(AppClaimsHelper.Authorization.Administration.All);
                if (_currentUser.CurrentCompanyId().HasValue)
                    claims.AddRange(AppClaimsHelper.Authorization.Company.All);
                if (_currentUser.CurrentBusinessUnitId().HasValue)
                    claims.AddRange(AppClaimsHelper.Authorization.BusinessUnit.All);
            }

            if (_currentUser.HasCompanyGodMode() || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyRolesManagement))
            {
                claims.AddRange(AppClaimsHelper.Authorization.Company.All);
                if (_currentUser.CurrentCompanyId().HasValue)
                    claims.AddRange(AppClaimsHelper.Authorization.Company.All);
                if (_currentUser.CurrentBusinessUnitId().HasValue)
                    claims.AddRange(AppClaimsHelper.Authorization.BusinessUnit.All);
            }

            if (_currentUser.HasBusinessUnitGodMode() || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitRolesManagement))
            {
                if (_currentUser.CurrentBusinessUnitId().HasValue)
                    claims.AddRange(AppClaimsHelper.Authorization.BusinessUnit.All);
            }

            return claims.Select(x => new SelectListItem
            {
                Value = x,
                Text = TranslationsHelper.Translate(x)
            }).ToList();
        }

        public List<SelectListItem> GetTicketStatuses()
        {
            return Enum.GetValues(typeof(TicketStatuses)).Cast<TicketStatuses>()
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = TranslationsHelper.Translate(x.ToString())
                }).ToList();
        }

        public List<SelectListItem> GetTicketTypes()
        {
            return Enum.GetValues(typeof(TicketTypes)).Cast<TicketTypes>()
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = TranslationsHelper.Translate(x.ToString())
                }).ToList();
        }
    }
}
