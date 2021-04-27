using System;

namespace BPWA.DAL.Models
{
    public interface IBaseDTO : IBaseDTO<int> { }

    public interface IBaseDTO<TKey>
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
