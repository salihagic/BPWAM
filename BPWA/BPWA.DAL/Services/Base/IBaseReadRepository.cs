using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    /// <summary>
    /// Interface that should be used per database entity,
    /// abstracts the persistance media eg. Repository pattern
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TSearchModel"></typeparam>
    /// <typeparam name="TDTO"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IBaseReadRepository<TEntity, TSearchModel, TDTO> :
        IBaseReadRespository<TEntity, TSearchModel, TDTO, int>
        where TEntity : class, IBaseEntity, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO
    { }

    /// <summary>
    /// Interface that should be used per database entity,
    /// abstracts the persistance media eg. Repository pattern
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TSearchModel"></typeparam>
    /// <typeparam name="TDTO"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IBaseReadRespository<TEntity, TSearchModel, TDTO, TId>
        where TEntity : class, IBaseEntity<TId>, new()
        where TSearchModel : IBaseSearchModel, new()
        where TDTO : IBaseDTO<TId>
    {
        /// <summary>
        /// Gets DTO by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mapped DTO from an entity</returns>
        Task<TDTO> GetById(TId id);

        /// <summary>
        /// Gets entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity by id</returns>
        Task<TEntity> GetEntityById(TId id);

        /// <summary>
        /// Method for filtering the data by SearchModel filters
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>A list of mapped entities to DTOs</returns>
        Task<List<TDTO>> Get(TSearchModel searchModel);

        /// <summary>
        /// Method for filtering the data by SearchModel filters
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>A list of entities</returns>
        Task<List<TEntity>> GetEntities(TSearchModel searchModel);
    }
}
