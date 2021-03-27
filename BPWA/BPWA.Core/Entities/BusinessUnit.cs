using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class BusinessUnit : BaseEntity, IBaseEntity
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }

        public Company Company { get; set; }
        public List<User> Users { get; set; }
        public List<BusinessUnitUser> BusinessUnitUsers { get; set; }
        public List<Role> Roles { get; set; }
    }
}
