namespace BPWA.DAL.Models
{
    public class UserLoginDTO : BaseDTO
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
