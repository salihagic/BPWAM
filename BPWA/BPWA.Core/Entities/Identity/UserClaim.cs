﻿using Microsoft.AspNetCore.Identity;
using System;

namespace BPWA.Core.Entities
{
    public class UserClaim : 
        IdentityUserClaim<string>, 
        IBaseEntity,
        IBaseAuditableEntity,
        IBaseCompanyEntity
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public int? CompanyId { get; set; }

        public Company Company { get; set; }
        public User User { get; set; }
    }
}
