namespace BPWA.Web.Services.Models
{
    public class UserUpdateModel : BaseUpdateModel<string>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? CityId { get; set; }
        public string SelectedCity { get; set; }
    }
}
