using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface ITranslationsService : IBaseCRUDService<Translation, TranslationSearchModel, TranslationDTO>
    {
        Task<T> Translate<T>(T element);
        Task<List<T>> Translate<T>(List<T> elements);
    }
}
