﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BPWA.Web.Services.Services
{
    //This is only a service for dropdowns like enumerations and fixed length lists(no database calls here)
    public interface IDropdownHelperService
    {
        List<SelectListItem> GetAppClaims();
        List<SelectListItem> GetTicketTypes();
        List<SelectListItem> GetTicketStatuses();
        List<SelectListItem> GetSystemLanguages();
        List<SelectListItem> GetNotificationTypes();
        List<SelectListItem> GetNotificationDistributionTypes();
    }
}
