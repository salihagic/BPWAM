﻿using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Group : BaseEntity, IBaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CompanyId { get; set; }
        public int? BusinessUnitId { get; set; }

        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public List<GroupUser> GroupUsers { get; set; }
        public List<NotificationGroup> NotificationGroups { get; set; }
    }
}
