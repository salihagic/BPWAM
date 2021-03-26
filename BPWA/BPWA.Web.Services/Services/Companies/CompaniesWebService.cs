using AutoMapper;
using BPWA.DAL.Database;

namespace BPWA.DAL.Services
{
    public class CompaniesWebService : CompaniesService, ICompaniesWebService
    {
        public CompaniesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            CurrentUser currentUser
            ) : base(databaseContext, mapper, currentUser)
        {
        }
    }
}
