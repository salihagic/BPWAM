using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Services;

namespace BPWA.Web.Services.Services
{
    public class AccountTypesWebService : AccountTypesService, IAccountTypesWebService
    {
        public AccountTypesWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }
    }
}
