using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;

namespace BPWA.Web.Services.Services
{
    public interface ICurrenciesWebService :
        IBaseCRUDWebService<Currency, CurrencySearchModel, CurrencyDTO, CurrencyAddModel, CurrencyUpdateModel>,
        ICurrenciesService
    {
    }
}
