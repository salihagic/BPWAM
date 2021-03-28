using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Services;

namespace BPWA.Web.Services.Services
{
    public class TicketsWebService : TicketsService, ITicketsWebService
    {
        public TicketsWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }
    }
}
