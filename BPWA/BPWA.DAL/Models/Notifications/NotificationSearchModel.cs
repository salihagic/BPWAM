using BPWA.Common.Enumerations;
using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class NotificationSearchModel : BaseSearchModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationDistributionType NotificationDistributionType { get; set; }
        public string UserId { get; set; }
        public List<int> NotificationGroupId { get; set; }
    }
}
