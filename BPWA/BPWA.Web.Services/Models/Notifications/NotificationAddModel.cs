using BPWA.Common.Enumerations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class NotificationAddModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationDistributionType NotificationDistributionType { get; set; }
        public string UserId { get; set; }
        public string SelectedUser { get; set; }
        public List<int> NotificationGroupIds { get; set; }
        public List<SelectListItem> NotificationGroupIdsSelectList { get; set; }
    }
}
