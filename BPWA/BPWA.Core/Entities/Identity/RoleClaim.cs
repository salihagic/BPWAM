using Microsoft.AspNetCore.Identity;
using System;

namespace BPWA.Core.Entities
{
    public class RoleClaim : IdentityRoleClaim<string>, IBaseEntity, IBaseSoftDeletableEntity
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
        public int? CompanyId { get; set; }

        public Company Company { get; set; }
        public Role Role { get; set; }
    }
}
