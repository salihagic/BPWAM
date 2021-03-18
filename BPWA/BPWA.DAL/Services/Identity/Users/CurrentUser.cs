using System.Collections.Generic;

namespace BPWA.DAL.Services
{
    public interface CurrentUser
    {
        string GetId();
        string GetUserName();
        string GetFirstName();
        string GetLastName();
        string GetFullName();
        bool HasClaim(string claim);
        List<string> GetConfiguration();
    }
}
