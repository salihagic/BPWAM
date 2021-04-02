using AutoMapper;
using BPWA.Common.Resources;
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

        public override Task<IActionResult> Index()
        {
            CurrentBreadcrumbItem = Translations._Translations;

            return base.Index();
        }
    }
}
