using System;

namespace BPWA.DAL.Models
{
    public interface IBaseAuditableDTO
    {
        public DateTime CreatedAtUtc { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
