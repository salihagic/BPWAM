using System;

namespace BPWA.DAL.Models
{
    public class BaseSoftDeletableAuditableDTO :
        BaseSoftDeletableAuditableDTO<int>,
        IBaseSoftDeletableDTO,
        IBaseAuditableDTO
        { }

    public class BaseSoftDeletableAuditableDTO<TKey> : 
        IBaseSoftDeletableDTO,
        IBaseAuditableDTO
    {
        public TKey Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
