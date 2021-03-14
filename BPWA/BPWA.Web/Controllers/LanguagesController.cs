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
    [Authorize(Policy = AppPolicies.LanguagesManagement)]
    public class LanguagesController :
        BaseCRUDController<
            Language,
            LanguageSearchModel,
            LanguageDTO,
            LanguageAddModel,
            LanguageUpdateModel
            >
    {
        public LanguagesController(
            ILanguagesWebService languagesWebService,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(languagesWebService, toast, mapper)
        { }
    }
}
