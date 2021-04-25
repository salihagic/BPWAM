using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

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

        public override IQueryable<Group> BuildQueryConditions(IQueryable<Group> Query, GroupSearchModel searchModel = null)
        {
            if (searchModel?.Title.IsNotEmpty() ?? false)
                searchModel.Title = searchModel.Title.ToLower();
            if (searchModel?.Description.IsNotEmpty() ?? false)
                searchModel.Description = searchModel.Description.ToLower();

            return base.BuildQueryConditions(Query, searchModel)
                       .WhereIf(searchModel?.Title.IsNotEmpty(), x => x.Title.ToLower().Contains(searchModel.Title))
                       .WhereIf(searchModel?.Description.IsNotEmpty(), x => x.Description.ToLower().Contains(searchModel.Description))
                       .WhereIf(searchModel?.UserId.IsNotEmpty(), x => x.GroupUsers.Any(y => y.UserId == searchModel.UserId));
        }
    }
}
