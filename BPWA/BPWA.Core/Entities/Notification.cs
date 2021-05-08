using BPWA.Common.Attributes;
using BPWA.Common.Enumerations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPWA.Core.Entities
{
    public class Notification : BaseEntity
    {
        [Translatable]
        public string Title { get; set; }
        [Translatable]
        public string Description { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationDistributionType NotificationDistributionType { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }
        [Translatable]
        public List<NotificationGroup> NotificationGroups { get; set; }
        public List<NotificationLog> NotificationLogs { get; set; }

        [NotMapped]
        public bool Seen { get; set; }
    }
}
