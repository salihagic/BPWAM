using AutoMapper;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Administration.CurrenciesManagement)]
    public class CurrenciesController :
        BaseCRUDController<
            Currency,
            CurrencySearchModel,
            CurrencyDTO,
            CurrencyAddModel,
            CurrencyUpdateModel
            >
    {
        public CurrenciesController(
            ICurrenciesWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, mapper, toast)
        { }
    }
}
