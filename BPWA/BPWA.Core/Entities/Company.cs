using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Company :
        BaseCompanyEntity, 
        IBaseEntity
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }
        public List<Company> Subcompanies { get; set; }
    }
}
