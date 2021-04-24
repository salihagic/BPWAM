using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public class GroupsService : BaseCRUDService<Group, GroupSearchModel, GroupDTO>, IGroupsService
    {
        public GroupsService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }
    }
}
