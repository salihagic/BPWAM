using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Services;

namespace BPWA.Web.Services.Services
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
