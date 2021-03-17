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
        List<string> GetConfiguration();
    }
}
