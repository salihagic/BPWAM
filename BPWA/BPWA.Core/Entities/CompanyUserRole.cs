namespace BPWA.Core.Entities
{
    public class CompanyUserRole : BaseEntity, IBaseEntity
    {
        public int CompanyUserId { get; set; }
        public string RoleId { get; set; }

        public CompanyUser CompanyUser { get; set; }
        public Role Role { get; set; }
    }
}
