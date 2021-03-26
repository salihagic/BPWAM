namespace BPWA.DAL.Models
{
    public class BusinessUnitUserDTO : BaseDTO
    {
        public int BusinessUnitId { get; set; }
        public string UserId { get; set; }

        public BusinessUnitDTO BusinessUnit { get; set; }
        public UserDTO User { get; set; }
    }
}
