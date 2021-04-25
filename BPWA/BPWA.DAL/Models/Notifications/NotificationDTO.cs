using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class NotificationDTO : BaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType NotificationType { get; set; }
        public string NotificationTypeString => TranslationsHelper.Translate(NotificationType.ToString());
        public NotificationDistributionType NotificationDistributionType { get; set; }
        public string NotificationDistributionTypeString => TranslationsHelper.Translate(NotificationDistributionType.ToString());
        public bool Seen { get; set; }

        public UserDTO User { get; set; }
        public List<NotificationGroupDTO> NotificationGroups { get; set; }
    }
}
