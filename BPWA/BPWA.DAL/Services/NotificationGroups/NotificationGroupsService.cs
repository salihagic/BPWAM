using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public class NotificationGroupsService : BaseCRUDService<NotificationGroup, NotificationGroupSearchModel, NotificationGroupDTO>, INotificationGroupsService
    {
        public NotificationGroupsService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }
    }
}
