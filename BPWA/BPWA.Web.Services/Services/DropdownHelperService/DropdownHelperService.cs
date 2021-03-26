using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Common.Security;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.DAL.Services
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

            if (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.GodMode))
            {
                claims.Add(AppClaims.Authorization.GodMode);
                claims.AddRange(AppClaimsHelper.Authorization.All);
            }
            else if (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Administration.RolesManagement))
            {
                claims.AddRange(AppClaimsHelper.Authorization.All);
            }
            else if (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.CompanyRolesManagement))
            {
                claims.AddRange(AppClaimsHelper.Authorization.Company.All);
                claims.AddRange(AppClaimsHelper.Authorization.BusinessUnit.All);
            } 
            else if (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.BusinessUnit.BusinessUnitRolesManagement))
            {
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
