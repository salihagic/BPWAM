using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Services;

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
    }
}
