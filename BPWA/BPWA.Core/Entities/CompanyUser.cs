namespace BPWA.Core.Entities
{
    public class CompanyUser : BaseEntity, IBaseEntity
    {
        public int CompanyId { get; set; }
        public string UserId { get; set; }

        public Company Company { get; set; }
        public User User { get; set; }
    }
}
