using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Role : 
        IdentityRole<string>, 
        IBaseEntity<string>,
        IBaseAuditableEntity,
        IBaseCompanyEntity,
        IBaseSoftDeletableCompanyEntity
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
        public int? CompanyId { get; set; }

        public Company Company { get; set; }
        public List<RoleClaim> RoleClaims { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
