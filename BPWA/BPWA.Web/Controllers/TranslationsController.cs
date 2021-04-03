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
using System.Collections.Generic;
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
        private ITranslationsWebService _translationsWebService;

        public TranslationsController(
            ITranslationsWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, mapper, toast)
        {
            _translationsWebService = service;
        }

        public override Task<IActionResult> Index()
        {
            CurrentBreadcrumbItem = Translations._Translations;

            return base.Index();
        }

        [HttpPost]
        public async Task<IActionResult> AddRange(List<TranslationAddModel> translations)
        {
            var result = await _translationsWebService.AddRange(translations);

            if (!result.IsSuccess)
                return BadRequest();

            return Ok();
        }
    }
}
