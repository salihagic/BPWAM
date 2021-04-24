using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;

namespace BPWA.Web.Services.Services
{
    public interface INotificationsWebService :
        IBaseCRUDWebService<Notification, NotificationSearchModel, NotificationDTO, NotificationAddModel, NotificationUpdateModel>,
        INotificationsService
    {
    }
}
