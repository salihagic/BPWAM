using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Company : BaseEntity, IBaseEntity
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
