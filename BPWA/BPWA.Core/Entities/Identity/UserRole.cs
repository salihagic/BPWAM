using Microsoft.AspNetCore.Identity;
using System;

namespace BPWA.Core.Entities
{
    public class UserRole : IdentityUserRole<string>, IBaseEntity, IBaseSoftDeletableEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
        public override string UserId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
