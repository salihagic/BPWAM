namespace BPWA.Core.Entities
{
    public class NotificationLog : BaseEntity, IBaseEntity
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public bool Seen { get; set; }

        public Notification Notification { get; set; }
        public User User { get; set; }
    }
}
