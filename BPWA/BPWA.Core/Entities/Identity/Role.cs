using BPWA.Common.Attributes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Role : IdentityRole<string>, IBaseEntity<string>, IBaseSoftDeletableEntity
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
        public int? CompanyId { get; set; }
        public int? BusinessUnitId { get; set; }
        [Translatable]
        public override string Name { get => base.Name; set => base.Name = value; }

        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }

        public List<RoleClaim> RoleClaims { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<CompanyUserRole> CompanyUserRoles { get; set; }
        public List<BusinessUnitUserRole> BusinessUnitUserRoles { get; set; }
    }
}
