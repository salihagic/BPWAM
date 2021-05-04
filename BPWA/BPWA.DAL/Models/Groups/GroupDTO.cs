using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class GroupDTO : BaseDTO, IBaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CompanyId { get; set; }
        public int? BusinessUnitId { get; set; }

        public CompanyDTO Company { get; set; }
        public BusinessUnitDTO BusinessUnit { get; set; }
        public List<GroupUserDTO> GroupUsers { get; set; }
        public List<NotificationGroupDTO> NotificationGroups { get; set; }
    }
}
