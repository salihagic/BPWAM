namespace BPWA.Web.Services.Models
{
    public class ResetPasswordModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
    }
}
