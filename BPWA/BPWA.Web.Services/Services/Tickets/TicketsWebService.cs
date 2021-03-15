using AutoMapper;
using BPWA.DAL.Database;

namespace BPWA.DAL.Services
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
