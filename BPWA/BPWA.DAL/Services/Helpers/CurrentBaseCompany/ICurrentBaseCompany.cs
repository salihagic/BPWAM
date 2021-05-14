using BPWA.Common.Enumerations;

namespace BPWA.DAL.Services
{
    public interface ICurrentBaseCompany
    {
        int? Id();
        bool IsGuest();
        AccountType? AccountType();
    }
}
