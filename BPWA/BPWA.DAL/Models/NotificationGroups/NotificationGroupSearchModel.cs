namespace BPWA.DAL.Models
{
    public class NotificationGroupSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public int NotificationId { get; set; }
        public int GroupId { get; set; }
    }
}
