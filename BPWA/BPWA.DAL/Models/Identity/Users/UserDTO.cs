namespace BPWA.DAL.Models
{
    public class UserDTO : BaseDTO<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TimezoneId { get; set; }
    }
}
