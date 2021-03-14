using AutoMapper;
using BPWA.DAL.Database;

namespace BPWA.DAL.Services
{
    public class LanguagesWebService : LanguagesService, ILanguagesWebService
    {
        public LanguagesWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }
    }
}
