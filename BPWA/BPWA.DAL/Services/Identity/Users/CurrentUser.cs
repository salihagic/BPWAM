using System.Collections.Generic;

namespace BPWA.DAL.Services
{
    public interface CurrentUser
    {
        string Id();
        string UserName();
        string FirstName();
        string LastName();
        string FullName();
        string TimezoneId();
        List<int> CompanyIds();
        int? CurrentCompanyId();
        string CurrentCompanyName();
        List<int> BusinessUnitIds();
        int? CurrentBusinessUnitId();
        string CurrentBusinessUnitName();
        bool HasAuthorizationClaim(string claim);
        bool HasGodMode();
        bool HasCompanyGodMode();
        bool HasBusinessUnitGodMode();
        List<string> Configuration();
    }
}
