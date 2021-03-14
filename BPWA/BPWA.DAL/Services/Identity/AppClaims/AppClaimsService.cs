using BPWA.Common.Extensions;
using BPWA.Common.Security;
using BPWA.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TFM.DAL.Models;

namespace BPWA.DAL.Services
{
    public class AppClaimsService : IAppClaimsService
    {
        virtual public Result<List<string>> Get(AppClaimsSearchModel searchModel = null)
        {
            try
            {
                var itemsQuery = AppClaimsHelper.Authorization.All
                    .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.ToLower().StartsWith(searchModel.Name.ToLower()));

                if (searchModel?.Pagination != null)
                    searchModel.Pagination.TotalNumberOfRecords = itemsQuery.Count();

                if (searchModel?.Pagination != null && !searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                    itemsQuery = itemsQuery.Skip(searchModel.Pagination.Skip.GetValueOrDefault())
                                           .Take(searchModel.Pagination.Take.GetValueOrDefault());

                var items = itemsQuery.ToList();

                return Result.Success(items);
            }
            catch (Exception e)
            {
                return Result.Failed<List<string>>("Failed to load app claims");
            }
        }
    }
}
