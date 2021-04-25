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
    public class NotificationsWebService : NotificationsService, INotificationsWebService
    {
        public NotificationsWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            CurrentUser currentUser
            ) : base(databaseContext, mapper, currentUser)
        {
        }

        public override IQueryable<Notification> BuildIncludesById(int id, IQueryable<Notification> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.User)
                       .Include(x => x.NotificationGroups)
                       .ThenInclude(x => x.Group);
        }

        public async Task<Result<NotificationAddModel>> PrepareForAdd(NotificationAddModel model = null)
        {
            model ??= new NotificationAddModel();

            if (model.GroupIds.IsNotEmpty())
            {
                try
                {
                    model.GroupIdsSelectList = await DatabaseContext.Currencies
                    .Where(x => model.GroupIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e) 
                {
                    return Result.Failed<NotificationAddModel>("Could not load groups");
                }
            }
            
            model.GroupIdsSelectList ??= new List<SelectListItem>();

            return Result.Success(model);
        }

        public async Task<Result<NotificationUpdateModel>> PrepareForUpdate(NotificationUpdateModel model = null)
        {
            model ??= new NotificationUpdateModel();

            if (model.GroupIds.IsNotEmpty())
            {
                try
                {
                    model.GroupIdsSelectList = await DatabaseContext.Groups
                    .Where(x => model.GroupIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Title }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<NotificationUpdateModel>("Could not load groups");
                }
            }

            model.GroupIdsSelectList ??= new List<SelectListItem>();

            return Result.Success(model);
        }

        public override async Task<Result<NotificationDTO>> Update(Notification entity)
        {
            var currentNotificationGroups = await DatabaseContext.NotificationGroups.Where(x => x.NotificationId == entity.Id).ToListAsync();

            if (currentNotificationGroups.IsNotEmpty())
            {
                //Delete
                var notificationGroupsToDelete = currentNotificationGroups.Where(x => !entity.NotificationGroups?.Any(y => y.GroupId == x.GroupId) ?? true).ToList();
                DatabaseContext.RemoveRange(notificationGroupsToDelete);
                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.NotificationGroups = entity.NotificationGroups.Where(x => !currentNotificationGroups.Any(y => y.GroupId == x.GroupId)).ToList();
            }

            return await base.Update(entity);
        }
    }
}
