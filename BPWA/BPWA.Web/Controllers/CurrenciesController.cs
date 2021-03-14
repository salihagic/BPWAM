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
    [Authorize(Policy = AppPolicies.CurrenciesManagement)]
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
            ICurrenciesWebService currenciesWebService,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(currenciesWebService, toast, mapper)
        { }
    }
}
