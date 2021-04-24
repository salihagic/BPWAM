namespace BPWA.Core.Entities
{
    public class NotificationGroup : BaseEntity, IBaseEntity
    {
        public int NotificationId { get; set; }
        public int GroupId { get; set; }

        public Notification Notification { get; set; }
        public Group Group { get; set; }
    }
}
