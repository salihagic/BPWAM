namespace BPWA.Core.Entities
{
    public class GroupUser : BaseEntity
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }

        public Group Group { get; set; }
        public User User { get; set; }
    }
}
