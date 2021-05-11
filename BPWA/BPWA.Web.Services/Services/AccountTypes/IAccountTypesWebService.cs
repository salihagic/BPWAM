using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;

namespace BPWA.Web.Services.Services
{
    public interface IAccountTypesWebService :
        IBaseCRUDWebService<AccountType, AccountTypeSearchModel, AccountTypeDTO, AccountTypeAddModel, AccountTypeUpdateModel>,
        IAccountTypesService
    {
    }
}
