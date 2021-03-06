using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class RoleSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string Name { get; set; }
        public List<string> Claims { get; set; }
    }
}
