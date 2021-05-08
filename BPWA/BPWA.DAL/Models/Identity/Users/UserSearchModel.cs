using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class UserSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TimezoneId { get; set; }
        public List<int> CityIds { get; set; }
        public List<string> RoleIds { get; set; }
        public List<int> CompanyIds { get; set; }
        public List<int> BusinessUnitIds { get; set; }
    }
}
