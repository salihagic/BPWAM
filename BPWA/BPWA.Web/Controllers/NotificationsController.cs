using AutoMapper;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Threading.Tasks;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Administration.NotificationsManagement)]
    public class NotificationsController :
        BaseCRUDController<
            Notification,
            NotificationSearchModel,
            NotificationDTO,
            NotificationAddModel,
            NotificationUpdateModel
            >
    {
        private INotificationsWebService _notificationsWebService;

        public NotificationsController(
            INotificationsWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, mapper, toast)
        {
            _notificationsWebService = service;
        }

        public async Task<IActionResult> GetUnseenNotificationsCountForCurrentUser()
        {
            var count = await _notificationsWebService.GetUnseenNotificationsCountForCurrentUser();

            return Ok(count);
        }

        [HttpPost]
        public async Task<IActionResult> GetDirect(NotificationSearchModel searchModel)
        {
            var notifications = await _notificationsWebService.GetDirectForCurrentUser(searchModel);

            return Ok(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> GetGroup(NotificationSearchModel searchModel)
        {
            var notifications = await _notificationsWebService.GetGroupForCurrentUser(searchModel);

            return Ok(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> GetBroadcast(NotificationSearchModel searchModel)
        {
            var notifications = await _notificationsWebService.GetBroadcastForCurrentUser(searchModel);

            return Ok(notifications);
        }
    }
}
