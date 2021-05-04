using AutoMapper;
using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class NotificationsService : BaseCRUDService<Notification, NotificationSearchModel, NotificationDTO>, INotificationsService
    {
        protected ICurrentUser _currentUser { get; }

        public NotificationsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentUser currentUser
            ) : base(databaseContext, mapper)
        {
            _currentUser = currentUser;
        }

        public override IQueryable<Notification> BuildQueryConditions(IQueryable<Notification> Query, NotificationSearchModel searchModel = null)
        {
            if (searchModel?.Title.IsNotEmpty() ?? false)
                searchModel.Title = searchModel.Title.ToLower();
            if (searchModel?.Description.IsNotEmpty() ?? false)
                searchModel.Description = searchModel.Description.ToLower();

            return base.BuildQueryConditions(Query, searchModel)
                       .WhereIf(searchModel?.Title.IsNotEmpty(), x => x.Title.ToLower().Contains(searchModel.Title))
                       .WhereIf(searchModel?.Description.IsNotEmpty(), x => x.Description.ToLower().Contains(searchModel.Description))
                       .WhereIf(searchModel?.NotificationType.HasValue, x => x.NotificationType == searchModel.NotificationType)
                       .WhereIf(searchModel?.NotificationDistributionType.HasValue, x => x.NotificationDistributionType == searchModel.NotificationDistributionType)
                       .WhereIf(searchModel?.UserId.IsNotEmpty(), x => x.UserId == searchModel.UserId)
                       .WhereIf(searchModel?.GroupId.HasValue, x => x.NotificationGroups.Any(y => y.GroupId == searchModel.GroupId));
        }

        public async Task<List<NotificationDTO>> GetForCurrentUser(NotificationSearchModel searchModel = null)
        {
            try
            {
                Query = Query
                    .Where(x => (x.NotificationDistributionType == NotificationDistributionType.SingleUser && x.UserId == _currentUser.Id()) ||
                                (x.NotificationDistributionType == NotificationDistributionType.Group && x.NotificationGroups.Any(y => y.Group.GroupUsers.Any(z => z.UserId == _currentUser.Id()))) ||
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

                await SetSeen(notifications);

                var notificationDTOs = Mapper.Map<List<NotificationDTO>>(notifications);

                return notificationDTOs;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load notifications");
            }
        }

        public async Task<List<NotificationDTO>> GetDirectForCurrentUser(NotificationSearchModel searchModel = null)
        {
            try
            {
                Query = Query
                    .Where(x => x.NotificationDistributionType == NotificationDistributionType.SingleUser && x.UserId == _currentUser.Id())
                    .Include(x => x.NotificationLogs)
                    .OrderByDescending(x => x.CreatedAtUtc);

                if (searchModel?.Pagination != null)
                {
                    searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

                    if (!searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                        Query = BuildQueryPagination(Query, searchModel);
                }

                var notifications = await Query.ToListAsync();

                await SetSeen(notifications);

                var notificationDTOs = Mapper.Map<List<NotificationDTO>>(notifications);

                return notificationDTOs;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load notifications");
            }
        }

        public async Task<List<NotificationDTO>> GetGroupForCurrentUser(NotificationSearchModel searchModel = null)
        {
            try
            {
                Query = Query
                    .Where(x => x.NotificationDistributionType == NotificationDistributionType.Group && x.NotificationGroups.Any(y => y.Group.GroupUsers.Any(z => z.UserId == _currentUser.Id())))
                    .Include(x => x.NotificationLogs)
                    .OrderByDescending(x => x.CreatedAtUtc);

                if (searchModel?.Pagination != null)
                {
                    searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

                    if (!searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                        Query = BuildQueryPagination(Query, searchModel);
                }

                var notifications = await Query.ToListAsync();

                await SetSeen(notifications);

                var notificationDTOs = Mapper.Map<List<NotificationDTO>>(notifications);

                return notificationDTOs;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load notifications");
            }
        }

        public async Task<List<NotificationDTO>> GetBroadcastForCurrentUser(NotificationSearchModel searchModel = null)
        {
            try
            {
                Query = Query
                    .Where(x => x.NotificationDistributionType == NotificationDistributionType.Broadcast)
                    .Include(x => x.NotificationLogs.Where(x => x.UserId == _currentUser.Id()))
                    .OrderByDescending(x => x.CreatedAtUtc);

                if (searchModel?.Pagination != null)
                {
                    searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

                    if (!searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                        Query = BuildQueryPagination(Query, searchModel);
                }

                var notifications = await Query.ToListAsync();

                await SetSeen(notifications);

                var notificationDTOs = Mapper.Map<List<NotificationDTO>>(notifications);

                return notificationDTOs;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load notifications");
            }
        }

        protected async Task SetSeen(List<Notification> notifications)
        {
            if (notifications.IsNotEmpty())
            {
                foreach (var notification in notifications)
                {
                    notification.Seen = notification.NotificationLogs.FirstOrDefault(x => x.NotificationId == notification.Id && x.UserId == _currentUser.Id())?.Seen ?? false;
                    notification.NotificationLogs.ForEach(x => x.Seen = true);
                }

                await DatabaseContext.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnseenNotificationsCountForCurrentUser()
        {
            try
            {
                var count = await DatabaseContext.NotificationLogs
                .Where(x => x.UserId == _currentUser.Id() && !x.Seen)
                .CountAsync();

                return count;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load notifications count");
            }
        }

        public override async Task<Notification> AddEntity(Notification entity)
        {
            var result = await base.AddEntity(entity);

            await UpdateNotificationLogs(result);

            return result;
        }

        public override async Task<Notification> UpdateEntity(Notification entity)
        {
            var result = await base.UpdateEntity(entity);

            await UpdateNotificationLogs(result);

            return result;
        }

        protected async Task UpdateNotificationLogs(Notification notification)
        {
            var notificationLogs = await DatabaseContext.NotificationLogs
                .Where(x => x.NotificationId == notification.Id)
                .ToListAsync();
            DatabaseContext.RemoveRange(notificationLogs);

            if (notification.NotificationDistributionType == NotificationDistributionType.SingleUser)
            {
                var user = await DatabaseContext.Notifications
                    .Where(x => x.Id == notification.Id)
                    .Select(x => x.User)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    await DatabaseContext.NotificationLogs.AddAsync(new NotificationLog
                    {
                        UserId = user.Id,
                        NotificationId = notification.Id
                    });
                    await DatabaseContext.SaveChangesAsync();
                }
            }
            if (notification.NotificationDistributionType == NotificationDistributionType.Group)
            {
                var userIds = await DatabaseContext.Groups
                    .Where(x => x.NotificationGroups.Any(y => y.GroupId == x.Id))
                    .SelectMany(x => x.GroupUsers)
                    .Select(x => x.UserId)
                    .ToListAsync();

                await DatabaseContext.NotificationLogs.AddRangeAsync(
                    userIds.Select(userId => new NotificationLog
                    {
                        UserId = userId,
                        NotificationId = notification.Id
                    }));
                await DatabaseContext.SaveChangesAsync();
            }
            if (notification.NotificationDistributionType == NotificationDistributionType.Broadcast)
            {
                var userIds = await DatabaseContext.Users
                    .Select(x => x.Id)
                    .ToListAsync();

                await DatabaseContext.NotificationLogs.AddRangeAsync(
                    userIds.Select(userId => new NotificationLog
                    {
                        UserId = userId,
                        NotificationId = notification.Id
                    }));
                await DatabaseContext.SaveChangesAsync();
            }
        }

        public override async Task<Notification> IncludeRelatedEntitiesToDelete(Notification entity)
        {
            return await DatabaseContext.Notifications
                .Include(x => x.NotificationLogs)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);
        }
    }
}
