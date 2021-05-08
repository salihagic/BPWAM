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

        public async Task<string> Translate(TranslationSearchModel model)
        {
            var translationCacheModel = new TranslationCacheModel
            {
                Culture = model.Culture,
                Key = model.Key,
                KeyHash = model.Key.GetHashString()
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

            return cacheEntry?.Value;
        }

        public async Task AddOrUpdateRange(List<Translation> entities)
        {
            foreach (var entity in entities)
            {
                var keyHash = entity.Key.GetHashString();

                var entityFromDatabase = await DatabaseContext.Translations
                    .FirstOrDefaultAsync(x => x.Culture == entity.Culture && x.KeyHash == keyHash) ??
                    new Translation
                    {
                        Culture = entity.Culture,
                        Key = entity.Key,
                        Value = entity.Value
                    };

                entityFromDatabase.Value = entity.Value;

                await UpdateEntity(entityFromDatabase);
            }

            return;
        }

        public override async Task<Translation> AddEntity(Translation entity)
        {
            entity.KeyHash = entity.Key.GetHashString();

            var result = await base.AddEntity(entity);

            var translationCacheModel = new TranslationCacheModel
            {
                Culture = entity.Culture,
                Key = entity.Key,
                KeyHash = entity.Key.GetHashString(),
                Value = entity.Value
            };

            _memoryCache.Set(translationCacheModel.CacheKey, translationCacheModel);

            return result;
        }

        public override async Task<Translation> UpdateEntity(Translation entity)
        {
            entity.KeyHash = entity.Key.GetHashString();

            var result = await base.UpdateEntity(entity);

            var translationCacheModel = new TranslationCacheModel
            {
                Culture = entity.Culture,
                Key = entity.Key,
                KeyHash = entity.Key.GetHashString(),
                Value = entity.Value
            };

            _memoryCache.Set(translationCacheModel.CacheKey, translationCacheModel);

            return result;
        }

        public override async Task Delete(int id)
        {
            var entityResult = await base.GetEntityById(id);

            await base.Delete(id);

            var entity = entityResult;

            var translationCacheModel = new TranslationCacheModel
            {
                Culture = entity.Culture,
                Key = entity.Key,
                KeyHash = entity.Key.GetHashString(),
                Value = entity.Value
            };

            _memoryCache.Remove(translationCacheModel.CacheKey);
        }

        private async Task<Dictionary<string, string>> GetTranslations(List<string> translationKeys)
        {
            var translations = new Dictionary<string, string>();

            foreach (var translationKey in translationKeys)
            {
                try
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
                catch (Exception e)
                {
                }
            }

            return translations;
        }
    }
}
