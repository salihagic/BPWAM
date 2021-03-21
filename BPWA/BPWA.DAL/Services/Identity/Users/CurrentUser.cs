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
        int? CompanyId();
        string CompanyName();
        int? BusinessUnitId();
        string BusinessUnitName();
        bool HasAuthorizationClaim(string claim);
        List<string> Configuration();
    }
}
