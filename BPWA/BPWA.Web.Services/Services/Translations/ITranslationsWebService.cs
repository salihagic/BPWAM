using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public interface ITranslationsWebService :
        IBaseCRUDWebService<Translation, TranslationSearchModel, TranslationDTO, TranslationAddModel, TranslationUpdateModel>,
        ITranslationsService
    {
        Task AddOrUpdateRange(List<TranslationAddModel> models);
    }
}
