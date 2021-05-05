using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        public override IQueryable<Group> BuildIncludesById(int id, IQueryable<Group> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.GroupUsers)
                       .ThenInclude(x => x.User);
        }

        public async Task<GroupDTO> Add(GroupAddModel model)
        {
            var entity = Mapper.Map<Group>(model);
            var result = await base.Add(entity);

            await ManageRelatedEntities<GroupUser, string>(result.Id, model.UserIds, x => x.GroupId, x => x.UserId);

            return result;
        }

        public async Task<GroupDTO> Update(GroupUpdateModel model)
        {
            var entity = await GetEntityById(model.Id, false, false);
            Mapper.Map(model, entity);
            var result = await base.Update(entity);

            await ManageRelatedEntities<GroupUser, string>(result.Id, model.UserIds, x => x.GroupId, x => x.UserId);

            return result;
        }
    }
}
