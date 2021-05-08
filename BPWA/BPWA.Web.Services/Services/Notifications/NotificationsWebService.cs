using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class NotificationsWebService : NotificationsService, INotificationsWebService
    {
        public NotificationsWebService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ICurrentUser currentUser,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, currentUser, translationsService)
        {
        }

        public override IQueryable<Notification> BuildIncludesById(int id, IQueryable<Notification> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.User)
                       .Include(x => x.NotificationGroups)
                       .ThenInclude(x => x.Group);
        }

        public async Task<NotificationDTO> Add(NotificationAddModel model)
        {
            var entity = Mapper.Map<Notification>(model);
            var result = await base.Add(entity);

            await ManageRelatedEntities<NotificationGroup>(result.Id, model.GroupIds, x => x.NotificationId, x => x.GroupId);

            return result;
        }

        public async Task<NotificationDTO> Update(NotificationUpdateModel model)
        {
            var entity = await GetEntityById(model.Id, false, false);
            Mapper.Map(model, entity);
            var result = await base.Update(entity);

            await ManageRelatedEntities<NotificationGroup>(result.Id, model.GroupIds, x => x.NotificationId, x => x.GroupId);

            return result;
        }
    }
}
