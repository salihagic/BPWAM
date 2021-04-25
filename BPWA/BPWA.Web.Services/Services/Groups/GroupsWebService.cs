using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Services;

namespace BPWA.Web.Services.Services
{
    public class GroupsWebService : GroupsService, IGroupsWebService
    {
        public GroupsWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }
    }
}
