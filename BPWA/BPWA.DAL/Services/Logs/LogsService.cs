using AutoMapper;
using AutoMapper.QueryableExtensions;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class LogsService : ILogsService
    {
        protected DatabaseContext DatabaseContext { get; set; }
        protected IQueryable<Log> Query { get; set; }
        protected IMapper Mapper { get; set; }

        public LogsService(
            DatabaseContext databaseContext,
            IMapper mapper
            )
        {
            DatabaseContext = databaseContext;
            Mapper = mapper;
            Query = databaseContext.Set<Log>().AsQueryable();
        }

        virtual public IQueryable<Log> BuildQueryConditions(IQueryable<Log> Query, LogSearchModel searchModel = null)
        {
            if (!string.IsNullOrEmpty(searchModel?.SearchTerm))
                searchModel.SearchTerm = $"%{searchModel.SearchTerm}%";

            return Query
                .WhereIf(!string.IsNullOrEmpty(searchModel?.SearchTerm), x => EF.Functions.ILike(x.Message, searchModel.SearchTerm));
        }

        virtual public IQueryable<Log> BuildQueryOrdering(IQueryable<Log> Query, LogSearchModel searchModel = null)
        {
            if (searchModel?.Pagination?.OrderFields.IsEmpty() ?? false)
                return Query.OrderByDescending(x => x.CreatedAt);

            var firstOrderField = searchModel.Pagination.OrderFields.First();

            if (string.Compare(firstOrderField.Field, nameof(LogDTO.CreatedAtString), true) == 0)
                Query = Query.OrderBy($"{nameof(LogDTO.CreatedAt)} {firstOrderField.Direction}");
            else
            {
                foreach (var orderField in searchModel.Pagination.OrderFields)
                    Query = Query.OrderBy($"{orderField.Field} {orderField.Direction}");
            }

            return Query;
        }

        virtual public IQueryable<Log> BuildQueryPagination(IQueryable<Log> Query, LogSearchModel searchModel = null)
        {
            if (searchModel?.Pagination == null)
                return Query;

            if (searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                return Query;

            return Query.Skip(searchModel.Pagination.Skip.GetValueOrDefault())
                        .Take(searchModel.Pagination.Take.GetValueOrDefault());
        }

        public async Task<List<LogDTO>>  Get(LogSearchModel searchModel = null)
        {

            Query = BuildQueryConditions(Query, searchModel);
            Query = BuildQueryOrdering(Query, searchModel);

            if (searchModel?.Pagination != null)
                searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

            if (searchModel?.Pagination != null && !searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                Query = BuildQueryPagination(Query, searchModel);

            return await Query
                .AsNoTracking()
                .ProjectTo<LogDTO>(Mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<LogDTO> GetById(int id)
        {
            return await Query.Where(x => x.Id.Equals(id))
                .AsNoTracking()
                .ProjectTo<LogDTO>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
