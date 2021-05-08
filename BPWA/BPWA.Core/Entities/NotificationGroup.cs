using BPWA.Common.Attributes;

namespace BPWA.Core.Entities
{
    public class NotificationGroup : BaseEntity
    {
        public int NotificationId { get; set; }
        public int GroupId { get; set; }

        public Notification Notification { get; set; }
        [Translatable]
        public Group Group { get; set; }
    }
}
