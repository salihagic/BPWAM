namespace BPWA.Core.Entities
{
    public class GroupUser :
        BaseCompanyEntity, 
        IBaseCompanyEntity, 
        IBaseEntity
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }

        public Group Group { get; set; }
        public User User { get; set; }
    }
}
