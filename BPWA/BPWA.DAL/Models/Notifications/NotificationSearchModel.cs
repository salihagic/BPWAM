using BPWA.Common.Enumerations;

namespace BPWA.DAL.Models
{
    public class NotificationSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType? NotificationType { get; set; }
        public NotificationDistributionType? NotificationDistributionType { get; set; }
        public string UserId { get; set; }
        public int? GroupId { get; set; }
    }
}
