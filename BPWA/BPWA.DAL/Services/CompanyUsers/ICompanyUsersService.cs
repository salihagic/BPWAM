using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ICompanyUsersService : 
        IBaseCRUDService<CompanyUser, CompanyUserSearchModel, CompanyUserDTO>
    {
    }
}
