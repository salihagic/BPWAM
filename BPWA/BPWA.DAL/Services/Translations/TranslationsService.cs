using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class TranslationsService : BaseCRUDService<Translation, TranslationSearchModel, TranslationDTO>, ITranslationsService
    {
        private IMemoryCache _memoryCache;
        private string _currentCulture => CultureInfo.CurrentCulture.Name;

        public TranslationsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            IMemoryCache memoryCache
            ) : base(databaseContext, mapper)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> Translate<T>(T element)
        {
            var translationKeys = element.GetTranslatableProps();

            var translations = await GetTranslations(translationKeys);

            return element.SetTranslatableProps(translations);
        }

        private async Task<Dictionary<string, string>> GetTranslations(List<string> translationKeys)
        {
            var translations = new Dictionary<string, string>();

            foreach (var translationKey in translationKeys)
            {
                var translationCacheModel = new TranslationCacheModel
                {
                    Culture = _currentCulture,
                    Key = translationKey,
                    KeyHash = translationKey.GetHashString()
                };

                var cacheEntry = await _memoryCache.GetOrCreateAsync(
                    translationCacheModel.CacheKey, 
                    async entry =>
                    {
                        entry.SlidingExpiration = TimeSpan.FromDays(5);

                        var translation = await DatabaseContext.Translations
                            .Where(x => x.KeyHash == translationCacheModel.KeyHash)
                            .Where(x => x.Culture == _currentCulture)
                            .FirstOrDefaultAsync();

                        return translation;
                    });

                if (cacheEntry != null)
                    translations.Add(cacheEntry.Key, cacheEntry.Value);
            }

            return translations;
        }

        public async Task<List<T>> Translate<T>(List<T> elements)
        {
            var translationKeys = elements.SelectMany(x => x.GetTranslatableProps()).ToList();

            var translations = await GetTranslations(translationKeys);

            foreach (var element in elements)
                element.SetTranslatableProps(translations);

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

    class TranslationCacheModel
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string KeyHash { get; set; }
        public string Value { get; set; }

        public string CacheKey => $"{KeyHash}-{Culture}";
    }
}
