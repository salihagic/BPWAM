using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class TicketsService
        : BaseTranslatableCRUDService<Ticket, TicketSearchModel, TicketDTO>, ITicketsService
    {
        public TicketsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            ITranslationsService translationsService
            ) : base(databaseContext, mapper, translationsService) { }

        public override IQueryable<Ticket> BuildQueryConditions(IQueryable<Ticket> query, TicketSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                .WhereIf(!string.IsNullOrEmpty(searchModel?.SearchTerm), x => x.Title.ToLower().StartsWith(searchModel.SearchTerm.ToLower()) || x.Description.ToLower().StartsWith(searchModel.SearchTerm.ToLower()))
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Title), x => x.Title.ToLower().StartsWith(searchModel.Title.ToLower()))
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Description), x => x.Description.ToLower().Contains(searchModel.Description.ToLower()))
                       .WhereIf(searchModel.TicketTypes.IsNotEmpty(), x => searchModel.TicketTypes.Contains(x.TicketType))
                       .WhereIf(searchModel.TicketStatuses.IsNotEmpty(), x => searchModel.TicketStatuses.Contains(x.TicketStatus));
        }
    }
}
