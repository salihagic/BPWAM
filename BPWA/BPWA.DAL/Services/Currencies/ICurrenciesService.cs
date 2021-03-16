using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ICurrenciesService : IBaseCRUDService<Currency, CurrencySearchModel, CurrencyDTO>
    {
    }
}
