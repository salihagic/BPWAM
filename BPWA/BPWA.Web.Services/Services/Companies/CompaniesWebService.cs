using AutoMapper;
using BPWA.DAL.Database;

namespace BPWA.DAL.Services
{
    public class CompaniesWebService : CompaniesService, ICompaniesWebService
    {
        private CurrentUser _currentUser;

        public CompaniesWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            CurrentUser currentUser
            ) : base(databaseContext, mapper)
        {
            _currentUser = currentUser;
        }
    }
}
