using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface INotificationsService : IBaseCRUDService<Notification, NotificationSearchModel, NotificationDTO>
    {
        Task<List<NotificationDTO>>  GetForCurrentUser(NotificationSearchModel searchModel = null);
        Task<List<NotificationDTO>>  GetDirectForCurrentUser(NotificationSearchModel searchModel = null);
        Task<List<NotificationDTO>>  GetGroupForCurrentUser(NotificationSearchModel searchModel = null);
        Task<List<NotificationDTO>>  GetBroadcastForCurrentUser(NotificationSearchModel searchModel = null);
        Task<int> GetUnseenNotificationsCountForCurrentUser();
    }
}
