using BPWA.Common.Enumerations;

namespace BPWA.DAL.Services
{
    public interface ICurrentUserBaseCompany
    {
        int? Id();
        bool IsGuest();
        AccountType? AccountType();
    }
}
