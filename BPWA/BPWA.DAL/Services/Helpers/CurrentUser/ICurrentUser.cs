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
        bool HasMultipleCompanies();
        int? CurrentCompanyId();
        string CurrentCompanyName();
        bool HasAuthorizationClaim(string claim);
        bool HasAdministrationAuthorizationClaim(string claim);
        bool HasCompanyAuthorizationClaim(string claim);
        bool HasGodMode();
        bool HasCompanyGodMode();
        List<string> Configuration();
    }
}
