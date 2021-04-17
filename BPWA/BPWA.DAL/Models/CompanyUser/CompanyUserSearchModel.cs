namespace BPWA.DAL.Models
{
    public class CompanyUserSearchModel : BaseSearchModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
