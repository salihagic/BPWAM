using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;

namespace BPWA.Web.Services.Services
{
    public interface ICompanyUsersWebService :
        IBaseCRUDWebService<CompanyUser, CompanyUserSearchModel, CompanyUserDTO, CompanyUserAddModel, CompanyUserUpdateModel>,
        ICompanyUsersService
    {
    }
}
