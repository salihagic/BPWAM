using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class RoleUpdateModel : BaseUpdateModel<string>
    {
        public string Name { get; set; }
        public List<string> Claims { get; set; }
    }
}
