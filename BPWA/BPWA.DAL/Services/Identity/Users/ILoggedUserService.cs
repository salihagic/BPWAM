using System.Collections.Generic;

namespace BPWA.DAL.Services
{
    public interface ILoggedUserService
    {
        string GetId();
        string GetUserName();
        string GetFirstName();
        string GetLastName();
        string GetFullName();
        List<string> GetConfiguration();
    }
}
