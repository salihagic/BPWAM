using AutoMapper;
using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BPWA.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.TicketsManagement)]
    public class TicketsController :
        BaseCRUDController<
            Ticket,
            TicketSearchModel,
            TicketDTO,
            TicketAddModel,
            TicketUpdateModel
            >
    {
        public TicketsController(
            ITicketsWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, toast, mapper)
        { }
    }
}
