using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ICurrenciesService : IBaseService<Currency, CurrencySearchModel, CurrencyDTO>
    {
    }
}
