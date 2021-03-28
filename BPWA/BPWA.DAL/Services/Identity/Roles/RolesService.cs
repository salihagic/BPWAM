using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public class RolesService : IRolesService
    {
        protected DatabaseContext DatabaseContext { get; set; }
        protected IQueryable<Role> Query { get; set; }
        protected IMapper Mapper { get; set; }
        protected RoleManager<Role> RoleManager { get; set; }

        public RolesService(
            DatabaseContext databaseContext,
            IMapper mapper,
            RoleManager<Role> roleManager
            )
        {
            DatabaseContext = databaseContext;
            Mapper = mapper;
            Query = databaseContext.Set<Role>().AsQueryable();
            RoleManager = roleManager;
        }

        public async Task<Role> GetEntityWithClaimsByName(string name)
        {
            var role = await RoleManager.FindByNameAsync(name);

            if (role == null)
                return null;

            role.RoleClaims = DatabaseContext.RoleClaims.Where(x => x.RoleId == role.Id).ToList();

            return role;
        }

        virtual public async Task<Result<RoleDTO>> Add(Role entity)
        {
            var result = await AddEntity(entity);

            if (!result.IsSuccess)
                return Result.Failed<RoleDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<RoleDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<RoleDTO>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<Role>> AddEntity(Role entity)
        {
            var exists = await RoleManager.RoleExistsAsync(entity.Name);

            if (exists)
                return Result.Failed<Role>("Role already exists");

            if (entity.Id == null)
                entity.Id = Guid.NewGuid().ToString();

            var identityResult = await RoleManager.CreateAsync(entity);

            if (!identityResult.Succeeded)
                return Result.Failed<Role>(identityResult.Errors.First().Description);

            return Result.Success(entity);
        }

        virtual public IQueryable<Role> BuildQueryConditions(IQueryable<Role> Query, RoleSearchModel searchModel = null)
        {
            if (searchModel == null)
                return Query;

            return Query
                .WhereIf(searchModel.IsDeleted.HasValue, x => x.IsDeleted == searchModel.IsDeleted.Value)
                .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.Name.ToLower().StartsWith(searchModel.Name.ToLower()))
                .WhereIf(searchModel.Claims.IsNotEmpty(), x => x.RoleClaims.Any(y => searchModel.Claims.Contains(y.ClaimValue) && !y.IsDeleted));
        }

        virtual public IQueryable<Role> BuildIncludesById(string id, IQueryable<Role> query) => query;

        virtual public IQueryable<Role> BuildIncludes(IQueryable<Role> query) => query;

        virtual public IQueryable<Role> BuildQueryOrdering(IQueryable<Role> Query, RoleSearchModel searchModel = null)
        {
            if (searchModel?.Pagination?.OrderFields == null)
                return Query;

            foreach (var orderField in searchModel.Pagination.OrderFields)
                Query = Query.OrderBy($"{orderField.Field} {orderField.Direction}");

            return Query;
        }

        virtual public IQueryable<Role> BuildQueryPagination(IQueryable<Role> Query, RoleSearchModel searchModel = null)
        {
            if (searchModel?.Pagination == null)
                return Query;

            if (searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                return Query;

            return Query.Skip(searchModel.Pagination.Skip.GetValueOrDefault())
                        .Take(searchModel.Pagination.Take.GetValueOrDefault());
        }

        virtual public async Task<Result<List<RoleDTO>>> Get(RoleSearchModel searchModel = null)
        {
            var result = await GetEntities(searchModel);

            if (!result.IsSuccess)
                return Result.Failed<List<RoleDTO>>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<List<RoleDTO>>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<List<RoleDTO>>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<List<Role>>> GetEntities(RoleSearchModel searchModel = null)
        {
            try
            {
                Query = BuildQueryConditions(Query, searchModel);
                Query = BuildQueryOrdering(Query, searchModel);

                if (searchModel?.Pagination != null)
                    searchModel.Pagination.TotalNumberOfRecords = await Query.CountAsync();

                if (searchModel?.Pagination != null && !searchModel.Pagination.ShouldTakeAllRecords.GetValueOrDefault())
                    Query = BuildQueryPagination(Query, searchModel);

                var items = await Query.AsNoTracking().ToListAsync();

                return Result.Success(items);
            }
            catch (Exception e)
            {
                return Result.Failed<List<Role>>("Failed to load entities");
            }
        }

        virtual public async Task<Result<RoleDTO>> GetById(string id)
        {
            var result = await GetEntityById(id);

            if (!result.IsSuccess)
                return Result.Failed<RoleDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<RoleDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<RoleDTO>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<Role>> GetEntityById(string id)
        {
            try
            {
                var query = DatabaseContext.Set<Role>().Where(x => x.Id.Equals(id));

                query = BuildIncludesById(id, query);

                var item = await query.AsNoTracking().FirstOrDefaultAsync();

                return Result.Success(item);
            }
            catch (Exception e)
            {
                return Result.Failed<Role>("Failed to load entity");
            }
        }

        virtual public async Task<Result<Role>> GetEntityByIdWithoutIncludes(string id)
        {
            try
            {
                var query = DatabaseContext.Roles.Where(x => x.Id.Equals(id));

                var item = await query.AsNoTracking().FirstOrDefaultAsync();

                return Result.Success(item);
            }
            catch (Exception e)
            {
                return Result.Failed<Role>("Failed to load entity");
            }
        }

        virtual public async Task<Result<RoleDTO>> Update(Role entity)
        {
            var result = await UpdateEntity(entity);

            if (!result.IsSuccess)
                return Result.Failed<RoleDTO>(result.GetErrorMessages());

            try
            {
                var mapped = Mapper.Map<RoleDTO>(result.Item);

                return Result.Success(mapped);
            }
            catch (Exception e)
            {
                return Result.Failed<RoleDTO>("Failed to map Entity to DTO");
            }
        }

        virtual public async Task<Result<Role>> UpdateEntity(Role entity)
        {
            try
            {
                DatabaseContext.Set<Role>().Update(entity);
                await DatabaseContext.SaveChangesAsync();

                return Result.Success(entity);
            }
            catch (Exception e)
            {
                return Result.Failed<Role>("Failed to update an item");
            }
        }

        virtual public async Task<Result> Delete(Role entity, bool softDelete = true)
        {
            try
            {
                if (softDelete)
                {
                    entity.IsDeleted = true;
                    await Update(entity);
                }
                else
                {
                    DatabaseContext.Set<Role>().Remove(entity);
                }

                await DatabaseContext.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failed("Failed to delete entity");
            }
        }

        virtual public async Task<Result> Delete(string id, bool softDelete = true)
        {
            var item = await DatabaseContext.Set<Role>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            return await Delete(item, softDelete);
        }
    }
}
