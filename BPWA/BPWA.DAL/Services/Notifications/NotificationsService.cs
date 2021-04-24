﻿using AutoMapper;
using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class NotificationsService : BaseCRUDService<Notification, NotificationSearchModel, NotificationDTO>, INotificationsService
    {
        protected CurrentUser CurrentUser { get; }

        public NotificationsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            CurrentUser currentUser
            ) : base(databaseContext, mapper)
        {
            CurrentUser = currentUser;
        }

        public async Task<Result<List<NotificationDTO>>> GetForCurrentUser(NotificationSearchModel searchModel = null)
        {
            try
            {
                Query = Query
                    .Where(x => (x.NotificationDistributionType == NotificationDistributionType.SingleUser && x.UserId == CurrentUser.Id()) ||
                                (x.NotificationDistributionType == NotificationDistributionType.Group && x.NotificationGroups.Any(y => y.Group.GroupUsers.Any(z => z.UserId == CurrentUser.Id()))) ||
                                (x.NotificationDistributionType == NotificationDistributionType.Broadcast))
                    .Include(x => x.NotificationLogs)
                    .OrderByDescending(x => x.CreatedAtUtc);

                if (searchModel?.Pagination != null)
                {
                    searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

                    if (!searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                        Query = BuildQueryPagination(Query, searchModel);
                }

                var notifications = await Query.ToListAsync();

                if (notifications.IsNotEmpty())
                {
                    foreach (var notification in notifications)
                    {
                        notification.Seen = notification.NotificationLogs.FirstOrDefault(x => x.NotificationId == notification.Id && x.UserId == CurrentUser.Id())?.Seen ?? false;
                        notification.NotificationLogs.ForEach(x => x.Seen = true);
                    }

                    await DatabaseContext.SaveChangesAsync();
                }

                var notificationDTOs = Mapper.Map<List<NotificationDTO>>(notifications);

                return Result.Success(notificationDTOs);
            }
            catch (Exception e)
            {
                return Result.Failed<List<NotificationDTO>>("Failed to load notifications");
            }
        }

        public async Task<Result<int>> GetUnseenNotificationsCountForCurrentUser()
        {
            try
            {
                var count = await DatabaseContext.NotificationLogs
                .Where(x => x.UserId == CurrentUser.Id() && !x.Seen)
                .CountAsync();

                return Result.Success(count);
            }
            catch (Exception e)
            {
                return Result.Failed<int>("Failed to load notifications count");
            }
        }
    }
}