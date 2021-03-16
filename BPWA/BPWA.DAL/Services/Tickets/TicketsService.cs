using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class TicketsService : BaseCRUDService<Ticket, TicketSearchModel, TicketDTO>, ITicketsService
    {
        public TicketsService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }

        public override IQueryable<Ticket> BuildQueryConditions(IQueryable<Ticket> query, TicketSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Title), x => x.Title.ToLower().StartsWith(searchModel.Title.ToLower()))
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Description), x => x.Description.ToLower().Contains(searchModel.Description.ToLower()))
                       .WhereIf(searchModel.TicketTypes.IsNotEmpty(), x => searchModel.TicketTypes.Contains(x.TicketType))
                       .WhereIf(searchModel.TicketStatuses.IsNotEmpty(), x => searchModel.TicketStatuses.Contains(x.TicketStatus));
        }
    }
}
