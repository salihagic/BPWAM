using BPWA.Web.Services.Models;
using System.Collections.Generic;

namespace BPWA.Web.Services.Services
{
    //This is only a service for dropdowns like enumerations and fixed length lists(no database calls here)
    public interface IDropdownHelperService
    {
        List<DropdownItem<string>> GetAppClaims();
        List<DropdownItem<string>> GetTicketTypes();
        List<DropdownItem<string>> GetTicketStatuses();
        List<DropdownItem<string>> GetSystemLanguages();
        List<DropdownItem<string>> GetAccountTypes();
        List<DropdownItem<string>> GetNotificationTypes();
        List<DropdownItem<string>> GetNotificationDistributionTypes();
        List<DropdownItem<string>> GetNotificationDistributionTypes1();
    }
}
