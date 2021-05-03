using BPWA.Common.Enumerations;
using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class NotificationAddModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType? NotificationType { get; set; }
        public NotificationDistributionType? NotificationDistributionType { get; set; }
        public string UserId { get; set; }
        public DropdownItem<string> UserIdDropdownItem { get; set; }
        public List<DropdownItem> GroupIdsDropdownItems { get; set; }
        public List<int> GroupIds => GroupIdsDropdownItems.GetIds();
    }
}
