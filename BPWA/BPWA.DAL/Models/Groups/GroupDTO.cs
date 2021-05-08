using BPWA.Common.Attributes;
using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class GroupDTO : BaseDTO, IBaseDTO
    {
        [Translatable]
        public string Title { get; set; }
        [Translatable]
        public string Description { get; set; }
        public int? CompanyId { get; set; }

        public CompanyDTO Company { get; set; }
        public List<GroupUserDTO> GroupUsers { get; set; }
        [Translatable]
        public List<NotificationGroupDTO> NotificationGroups { get; set; }
    }
}
