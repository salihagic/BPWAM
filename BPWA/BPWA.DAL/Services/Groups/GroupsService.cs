using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class GroupsService :
        BaseTranslatableCRUDService<Group, GroupSearchModel, GroupDTO>, IGroupsService
    {
        public GroupsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, translationsService)
        {
        }

        public override IQueryable<Group> BuildQueryConditions(IQueryable<Group> Query, GroupSearchModel searchModel = null)
        {
            if (searchModel?.Title.IsNotEmpty() ?? false)
                searchModel.Title = searchModel.Title.ToLower();
            if (searchModel?.Description.IsNotEmpty() ?? false)
                searchModel.Description = searchModel.Description.ToLower();

            return base.BuildQueryConditions(Query, searchModel)
                       .WhereIf(!string.IsNullOrEmpty(searchModel?.SearchTerm), x => x.Title.ToLower().StartsWith(searchModel.SearchTerm.ToLower()) || x.Description.ToLower().StartsWith(searchModel.SearchTerm.ToLower()))
                       .WhereIf(searchModel?.Title.IsNotEmpty(), x => x.Title.ToLower().Contains(searchModel.Title))
                       .WhereIf(searchModel?.Description.IsNotEmpty(), x => x.Description.ToLower().Contains(searchModel.Description))
                       .WhereIf(searchModel?.UserId.IsNotEmpty(), x => x.GroupUsers.Any(y => y.UserId == searchModel.UserId));
        }
    }
}
