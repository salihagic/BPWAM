using System;

namespace BPWA.DAL.Models
{
    public class BaseDTO : 
        BaseDTO<int>,
        IBaseDTO,
        IBaseSoftDeletableDTO,
        IBaseAuditableDTO,
        IBaseCompanyDTO
    { }

    public class BaseDTO<TKey> : 
        IBaseDTO<TKey>,
        IBaseSoftDeletableDTO,
        IBaseAuditableDTO,
        IBaseCompanyDTO
    {
        public TKey Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int? CompanyId { get; set; }

        public CompanyDTO Company { get; set; }
    }
}
