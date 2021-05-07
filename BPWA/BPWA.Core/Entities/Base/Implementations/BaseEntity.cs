using System;

namespace BPWA.Core.Entities
{
    public class BaseEntity : 
        BaseEntity<int> { }

    public class BaseEntity<TKey> : 
        IBaseEntity<TKey>,
        IBaseSoftDeletableEntity,
        IBaseAuditableEntity,
        IBaseCompanyEntity
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
