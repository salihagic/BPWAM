namespace BPWA.Core.Entities
{
    public class CompanyUser : BaseEntity, IBaseEntity
    {
        public string UserId { get; set; }
        public int CompanyId { get; set; }

        public User User { get; set; }
        public Company Company { get; set; }
    }
}
