using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<Result<GroupAddModel>> PrepareForAdd(GroupAddModel model = null)
        {
            model ??= new GroupAddModel();

            if (model.UserIds.IsNotEmpty())
            {
                try
                {
                    model.UserIdsSelectList = await DatabaseContext.Users
                    .Where(x => model.UserIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.FirstName} {x.LastName}" }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<GroupAddModel>("Could not load users");
                }
            }

            model.UserIdsSelectList ??= new List<SelectListItem>();

            return Result.Success(model);
        }

        public async Task<Result<GroupUpdateModel>> PrepareForUpdate(GroupUpdateModel model = null)
        {
            model ??= new GroupUpdateModel();

            if (model.UserIds.IsNotEmpty())
            {
                try
                {
                    model.UserIdsSelectList = await DatabaseContext.Users
                    .Where(x => model.UserIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.FirstName} {x.LastName}" }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<GroupUpdateModel>("Could not load currencies");
                }
            }
            
            model.UserIdsSelectList ??= new List<SelectListItem>();

            return Result.Success(model);
        }

        public override async Task<Result<GroupDTO>> Update(Group entity)
        {
            var currentGroupUsers = await DatabaseContext.GroupUsers.Where(x => x.GroupId == entity.Id).ToListAsync();

            if (currentGroupUsers.IsNotEmpty())
            {
                //Delete
                var groupUsersToDelete = currentGroupUsers.Where(x => !entity.GroupUsers?.Any(y => y.UserId == x.UserId) ?? true).ToList();
                DatabaseContext.RemoveRange(groupUsersToDelete);
                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.GroupUsers = entity.GroupUsers.Where(x => !currentGroupUsers.Any(y => y.UserId == x.UserId)).ToList();
            }

            return await base.Update(entity);
        }
    }
}
