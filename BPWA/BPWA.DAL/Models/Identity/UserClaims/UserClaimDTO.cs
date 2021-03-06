namespace BPWA.DAL.Models
{
    public class UserClaimDTO : 
        BaseDTO, 
        IBaseDTO
    {
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
