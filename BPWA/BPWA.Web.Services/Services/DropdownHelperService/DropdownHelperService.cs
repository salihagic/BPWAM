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
        virtual public List<SelectListItem> GetAppClaims()
        {
            return AppClaimsHelper.Authorization.All
                .Select(x => new SelectListItem
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
