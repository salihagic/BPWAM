using System.Collections.Generic;
using System.Linq;

namespace BPWA.Web.Helpers.Routing
{
    public static class Areas
    {
        public const string Auth = nameof(Auth);
        public const string Administration = nameof(Administration);
        public const string Companies = nameof(Companies);
        public const string BusinessUnits = nameof(BusinessUnits);
    }

    public static class AreasHelper
    {
        public static List<string> All => typeof(Areas).GetFields().Select(x => x.Name).ToList();
    }
}
