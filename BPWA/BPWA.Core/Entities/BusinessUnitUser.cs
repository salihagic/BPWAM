namespace BPWA.Core.Entities
{
    public class BusinessUnitUser : BaseEntity, IBaseEntity
    {
        public int BusinessUnitId { get; set; }
        public string UserId { get; set; }

        public BusinessUnit BusinessUnit { get; set; }
        public User User { get; set; }
    }
}
