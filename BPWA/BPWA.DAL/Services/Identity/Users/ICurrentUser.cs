using System.Collections.Generic;

namespace BPWA.DAL.Services
{
    public interface ICurrentUser
    {
        string GetId();
        string GetUserName();
        string GetFirstName();
        string GetLastName();
        string GetFullName();
        List<string> GetConfiguration();
    }
}
