using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Enumerations;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class CompanyActivityStatusLogsService :
        BaseCRUDService<CompanyActivityStatusLog, CompanyActivityStatusLogSearchModel, CompanyActivityStatusLogDTO>,
        ICompanyActivityStatusLogsService
    {
        private IMemoryCache _memoryCache;
        private CacheSettings _cacheSettings;
        private IHttpClientFactory _clientFactory;
        private RouteSettings _routeSettings;

        public CompanyActivityStatusLogsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            IMemoryCache memoryCache,
            CacheSettings cacheSettings,
            IHttpClientFactory clientFactory,
            RouteSettings routeSettings
            ) : base(databaseContext, mapper)
        {
            _memoryCache = memoryCache;
            _cacheSettings = cacheSettings;
            _clientFactory = clientFactory;
            _routeSettings = routeSettings;
        }

        public async Task<CompanyActivityStatusCacheModel> GetLastByCompanyId(int companyId)
        {
            var cacheModel = new CompanyActivityStatusCacheModel
            {
                CompanyId = companyId
            };

            var cacheEntry = await _memoryCache.GetOrCreateAsync(
                cacheModel.CacheKey,
                async entry =>
                {
                    entry.SlidingExpiration = _cacheSettings.CompanyActivityStatusDuration;

                    var entity = await GetLastEntityByCompanyId(companyId);

                    return Mapper.Map<CompanyActivityStatusCacheModel>(entity) ?? new CompanyActivityStatusCacheModel
                    {
                        CompanyId = companyId,
                        ActivityStatus = ActivityStatus.Active
                    };
                });

            return cacheEntry;
        }

        protected async Task<CompanyActivityStatusLog> GetLastEntityByCompanyId(int companyId)
        {
            return await DatabaseContext.CompanyActivityStatusLogs
                .IgnoreQueryFilters()
                .Where(x => x.CompanyId == companyId)
                .OrderBy(x => x.CreatedAtUtc)
                .LastOrDefaultAsync();
        }

        public async Task<bool> IsActive(int companyId)
        {
            var cacheModel = await GetLastByCompanyId(companyId);

            return cacheModel.ActivityStatus == ActivityStatus.Active;
        }

        public override async Task<CompanyActivityStatusLog> AddEntity(CompanyActivityStatusLog entity)
        {
            var result = await base.AddEntity(entity);

            await NotifyClientsForCacheUpdate(entity.CompanyId);

            return result;
        }

        public async Task NotifyClientsForCacheUpdate(int companyId)
        {
            //Refresh cache item for WEB application
            await RefreshCacheByCompanyId(companyId);

            //Notify API to refresh the cache item
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_routeSettings.ApiUrl}{_routeSettings.ApiCacheUpdateUrl}");
            var client = _clientFactory.CreateClient();
            await client.SendAsync(request);
        }

        public async Task RefreshCacheByCompanyId(int companyId)
        {
            var entity = await GetLastEntityByCompanyId(companyId);

            var cacheModel = Mapper.Map<CompanyActivityStatusCacheModel>(entity) ?? new CompanyActivityStatusCacheModel
            {
                CompanyId = companyId
            };

            if (entity == null)
                _memoryCache.Remove(cacheModel.CacheKey);
            else
                _memoryCache.Set(cacheModel.CacheKey, cacheModel);
        }
    }
}
