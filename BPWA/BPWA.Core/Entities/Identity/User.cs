using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class User : IdentityUser<string>, IBaseEntity<string>, IBaseSoftDeletableEntity
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TimezoneId { get; set; }
        public int? CityId { get; set; }
        public int? CurrentCompanyId { get; set; }
        public int? CurrentBusinessUnitId { get; set; }

        public City City { get; set; }
        public Company CurrentCompany { get; set; }
        public BusinessUnit CurrentBusinessUnit { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<UserClaim> UserClaims { get; set; }
        public List<UserLogin> UserLogins { get; set; }
        public List<UserToken> UserTokens { get; set; }
    }
}
