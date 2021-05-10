using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class CompanyDTO :
        BaseDTO,
        IBaseDTO
    {
        public string Name { get; set; }

        public List<UserDTO> Users { get; set; }
        public List<CompanyDTO> Subcompanies { get; set; }
    }
}
