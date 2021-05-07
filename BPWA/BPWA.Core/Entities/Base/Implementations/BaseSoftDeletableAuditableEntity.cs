using System;

namespace BPWA.Core.Entities
{
    public class BaseSoftDeletableAuditableEntity : 
        BaseSoftDeletableAuditableEntity<int> { }

    public class BaseSoftDeletableAuditableEntity<TKey> : 
        IBaseEntity<TKey>,
        IBaseSoftDeletableEntity,
        IBaseAuditableEntity
    {
        public TKey Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
