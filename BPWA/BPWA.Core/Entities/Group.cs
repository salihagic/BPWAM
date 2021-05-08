using BPWA.Common.Attributes;
using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Group : BaseEntity
    {
        [Translatable]
        public string Title { get; set; }
        [Translatable]
        public string Description { get; set; }

        public List<GroupUser> GroupUsers { get; set; }
        public List<NotificationGroup> NotificationGroups { get; set; }
    }
}
