using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class BusinessUnitDTO : BaseDTO
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }

        public CompanyDTO Company { get; set; }
        public List<UserDTO> Users { get; set; }
        public List<RoleDTO> Roles { get; set; }
    }
}
