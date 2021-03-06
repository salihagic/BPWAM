using BPWA.Common.Attributes;

namespace BPWA.DAL.Models
{
    public class NotificationGroupDTO : 
        BaseDTO, 
        IBaseDTO
    {
        public int NotificationId { get; set; }
        public int GroupId { get; set; }

        public NotificationDTO Notification { get; set; }
        [Translatable]
        public GroupDTO Group { get; set; }
    }
}
