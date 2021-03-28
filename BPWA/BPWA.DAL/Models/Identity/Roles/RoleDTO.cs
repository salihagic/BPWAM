using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class RoleDTO : BaseDTO<string>
    {
        public string Name { get; set; }

        public CompanyDTO Company { get; set; }
        public BusinessUnitDTO BusinessUnit { get; set; }
        public List<RoleClaimDTO> RoleClaims { get; set; }
    }
}
