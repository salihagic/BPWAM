using AutoMapper;
using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BPWA.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.CompaniesManagement)]
    public class CompaniesController :
        BaseCRUDController<
            Company,
            CompanySearchModel,
            CompanyDTO,
            CompanyAddModel,
            CompanyUpdateModel
            >
    {
        public CompaniesController(
            ICompaniesWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, toast, mapper)
        { }
    }
}
