using System;

namespace BPWA.Core.Entities
{
    public interface IBaseAuditableEntity
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
    }
}
