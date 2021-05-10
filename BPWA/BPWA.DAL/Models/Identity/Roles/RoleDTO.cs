using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class RoleDTO : 
        BaseDTO<string>, 
        IBaseDTO<string>
    {
        public string Name { get; set; }

        public List<RoleClaimDTO> RoleClaims { get; set; }
    }
}
