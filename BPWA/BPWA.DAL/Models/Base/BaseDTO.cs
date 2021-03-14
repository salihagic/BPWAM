using System;

namespace BPWA.DAL.Models
{
    public class BaseDTO : BaseDTO<int> {}

    public class BaseDTO<T>
    {
        public T Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
