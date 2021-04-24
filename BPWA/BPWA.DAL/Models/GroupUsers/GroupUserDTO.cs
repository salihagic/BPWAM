namespace BPWA.DAL.Models
{
    public class GroupUserDTO : BaseDTO
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }

        public GroupDTO Group { get; set; }
        public UserDTO User { get; set; }
    }
}
