namespace BPWA.DAL.Models
{
    public class UserRoleDTO : BaseDTO, IBaseDTO
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public UserDTO User { get; set; }
        public RoleDTO Role { get; set; }
    }
}
