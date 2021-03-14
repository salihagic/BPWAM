using System.Collections.Generic;
using System.Linq;

namespace BPWA.Common.Security
{
    public static class AppRoles
    {
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string CompanyAdmin = nameof(CompanyAdmin);
        public const string BusinessUnitAdmin = nameof(BusinessUnitAdmin);
        public const string User = nameof(User);
    }

    public static class AppRolesHelper
    {
        public static List<string> All => typeof(AppRoles).GetFields().Select(x => x.Name).ToList();
    }
}
