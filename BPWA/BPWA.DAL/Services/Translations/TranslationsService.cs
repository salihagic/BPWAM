using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Models.Translations;
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

        public async Task<List<T>> Translate<T>(List<T> elements)
        {
            var translationKeys = elements.SelectMany(x => x.GetTranslatableProps()).ToList();

            var translations = await GetTranslations(translationKeys);

            foreach (var element in elements)
                element.SetTranslatableProps(translations);

            return elements;
        }

        public override async Task<Result<Translation>> AddEntity(Translation entity)
        {
            entity.KeyHash = entity.Key.GetHashString();

            var result = await base.AddEntity(entity);

            if (result.IsSuccess)
            {
                var translationCacheModel = new TranslationCacheModel
                {
                    Culture = entity.Culture,
                    Key = entity.Key,
                    KeyHash = entity.Key.GetHashString(),
                    Value = entity.Value
                };

                _memoryCache.Set(translationCacheModel.CacheKey, translationCacheModel);
            }

            return result;
        }

        public override async Task<Result<Translation>> UpdateEntity(Translation entity)
        {
            entity.KeyHash = entity.Key.GetHashString();

            var result = await base.UpdateEntity(entity);

            if (result.IsSuccess)
            {
                var translationCacheModel = new TranslationCacheModel
                {
                    Culture = entity.Culture,
                    Key = entity.Key,
                    KeyHash = entity.Key.GetHashString(),
                    Value = entity.Value
                };

                _memoryCache.Set(translationCacheModel.CacheKey, translationCacheModel);
            }

            return result;
        }

        public override async Task<Result> Delete(int id, bool softDelete = true)
        {
            var entityResult = await base.GetEntityById(id);

            if (!entityResult.IsSuccess)
                return Result.Failed("Failed to load translation");

            var result = await base.Delete(id, false);

            if (result.IsSuccess)
            {
                var entity = entityResult.Item;

                var translationCacheModel = new TranslationCacheModel
                {
                    Culture = entity.Culture,
                    Key = entity.Key,
                    KeyHash = entity.Key.GetHashString(),
                    Value = entity.Value
                };

                _memoryCache.Remove(translationCacheModel.CacheKey);
            }

            return result;
        }

        private async Task<Dictionary<string, string>> GetTranslations(List<string> translationKeys)
        {
            var translations = new Dictionary<string, string>();

            try
            {

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
                                .Select(x => new TranslationCacheModel
                                {
                                    Culture = x.Culture,
                                    Key = x.Key,
                                    KeyHash = x.KeyHash,
                                    Value = x.Value
                                })
                                .FirstOrDefaultAsync();

                            return translation;
                        });

                    if (cacheEntry != null)
                        translations.Add(cacheEntry.Key, cacheEntry.Value);
                }

                return translations;
            }
            catch (Exception e)
            {
            }

            return translations;
        }
    }
}
