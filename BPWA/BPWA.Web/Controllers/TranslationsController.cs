using AutoMapper;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BPWA.Administration.Controllers
{
    [Authorize]
    public class TranslationsController :
        BaseCRUDController<
            Translation,
            TranslationSearchModel,
            TranslationDTO,
            TranslationAddModel,
            TranslationUpdateModel
            >
    {
        public TranslationsController(
            ITranslationsWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, mapper, toast)
        { }
    }
}
