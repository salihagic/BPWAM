namespace BPWA.DAL.Models
{
    public class CompanyUserDTO : BaseDTO, IBaseDTO
    {
        public int CompanyId { get; set; }
        public string UserId { get; set; }

        public CompanyDTO Company { get; set; }
        public UserDTO User { get; set; }
    }
}
