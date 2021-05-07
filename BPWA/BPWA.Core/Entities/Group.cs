using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Group : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<GroupUser> GroupUsers { get; set; }
        public List<NotificationGroup> NotificationGroups { get; set; }
    }
}
