namespace BPWA.Core.Entities
{
    public class BusinessUnitUserRole : BaseEntity, IBaseEntity
    {
        public int BusinessUnitUserId { get; set; }
        public string RoleId { get; set; }

        public BusinessUnitUser BusinessUnitUser { get; set; }
        public Role Role { get; set; }
    }
}
