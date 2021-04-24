using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class GroupDTO : BaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<GroupUserDTO> GroupUsers { get; set; }
    }
}
