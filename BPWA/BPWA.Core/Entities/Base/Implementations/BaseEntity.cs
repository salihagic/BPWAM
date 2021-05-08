using System;

namespace BPWA.Core.Entities
{
    public class BaseEntity : 
        BaseEntity<int>,
        IBaseEntity,
        IBaseCompanyEntity,
        IBaseAuditableEntity,
        IBaseSoftDeletableEntity,
        IBaseSoftDeletableCompanyEntity,
        IBaseSoftDeletableAuditableEntity
    { }

    public class BaseEntity<TKey> : 
        IBaseEntity<TKey>,
        IBaseCompanyEntity,
        IBaseAuditableEntity,
        IBaseSoftDeletableEntity,
        IBaseSoftDeletableCompanyEntity,
        IBaseSoftDeletableAuditableEntity
    {
        public TKey Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
        public int? CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
