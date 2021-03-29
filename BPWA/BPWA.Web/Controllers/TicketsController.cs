using AutoMapper;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Helpers.Filters;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Threading.Tasks;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Administration.TicketsManagement)]
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
            base(service, mapper, toast)
        { }

        [AllowAnonymous]
        public override async Task<IActionResult> Add(bool fullPage = false) => await base.Add(fullPage);

        [HttpPost, AllowAnonymous, Transaction]
        public override async Task<IActionResult> Add(TicketAddModel model) => await base.Add(model);
    }
}
