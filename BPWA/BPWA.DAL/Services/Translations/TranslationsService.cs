using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BPWA.Common.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BPWA.DAL.Services
{
    public class TranslationsService : BaseCRUDService<Translation, TranslationSearchModel, TranslationDTO>, ITranslationsService
    {
        public TranslationsService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }

        public async Task<T> Translate<T>(T element)
        {
            var translationKeys = element.GetTranslatableProps().ToHashedList();

            var translations = (await DatabaseContext.Translations
                .Where(x => translationKeys.Contains(x.KeyHash))
                .ToDictionaryAsync(x => x.Key, x => x.Value));

            return element.SetTranslatableProps(translations);
        }

        public async Task<List<T>> Translate<T>(List<T> elements)
        {
            foreach (var element in elements)
                await Translate(element);

            return elements;
        }

        public override Task<Result<Translation>> AddEntity(Translation entity)
        {
            entity.KeyHash = entity.Key.GetHashString();

            return base.AddEntity(entity);
        }

        public override Task<Result<Translation>> UpdateEntity(Translation entity)
        {
            entity.KeyHash = entity.Key.GetHashString();

            return base.AddEntity(entity);
        }
    }
}
