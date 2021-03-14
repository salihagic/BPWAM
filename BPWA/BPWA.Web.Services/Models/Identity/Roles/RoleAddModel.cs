using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class RoleAddModel
    {
        public string Name { get; set; }
        public List<string> Claims { get; set; } = new List<string>();
    }
}
