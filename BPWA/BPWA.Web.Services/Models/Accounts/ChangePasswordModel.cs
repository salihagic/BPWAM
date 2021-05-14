namespace BPWA.Web.Services.Models
{
    public class ChangePasswordModel
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmed { get; set; }
    }
}
