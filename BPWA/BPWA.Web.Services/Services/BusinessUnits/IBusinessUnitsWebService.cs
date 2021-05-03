using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public interface IBusinessUnitsWebService :
        IBaseCRUDWebService<BusinessUnit, BusinessUnitSearchModel, BusinessUnitDTO, BusinessUnitAddModel, BusinessUnitUpdateModel>,
        IBusinessUnitsService
    {
        Task<List<BusinessUnitDTO>> GetForCurrentUser();
    }
}
