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
    [Authorize(Policy = AppClaims.Authorization.LanguagesManagement)]
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
            ILanguagesWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, toast, mapper)
        { }
    }
}
