using System.Collections.Generic;

namespace BPWA.DAL.Services
{
    public interface ICurrentUser
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
        bool HasAuthorizationClaim(string claim);
        bool HasGodMode();
        bool HasCompanyGodMode();
        List<string> Configuration();
    }
}
