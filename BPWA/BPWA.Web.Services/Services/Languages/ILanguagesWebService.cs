using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;

namespace BPWA.DAL.Services
{
    public interface ILanguagesWebService :
        IBaseWebService<Language, LanguageSearchModel, LanguageDTO, LanguageAddModel, LanguageUpdateModel>,
        ILanguagesService
    {
    }
}
