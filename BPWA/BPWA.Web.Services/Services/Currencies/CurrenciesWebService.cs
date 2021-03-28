using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Services;

namespace BPWA.Web.Services.Services
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
