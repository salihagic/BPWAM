using AutoMapper;
using BPWA.DAL.Database;

namespace BPWA.DAL.Services
{
    public class CurrenciesWebService : CurrenciesService, ICurrenciesWebService
    {
        public CurrenciesWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }
    }
}
