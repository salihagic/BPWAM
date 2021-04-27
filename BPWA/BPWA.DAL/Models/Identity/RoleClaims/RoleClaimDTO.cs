namespace BPWA.DAL.Models
{
    public class RoleClaimDTO : BaseDTO, IBaseDTO
    {
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
