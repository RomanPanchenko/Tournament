using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // CREATE

        /// <summary>
        ///     Adds entity to database
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        ///     Adds range of entities to database
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        // READ

        /// <summary>
        ///     Gets entity from database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        TEntity GetById(int id);

        /// <summary>
        ///     Gets all entities from database
        /// </summary>
        /// <returns>List of entities</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        ///     Gets all entities from database sorted by sort condition
        /// </summary>
        /// <param name="sortCondition"></param>
        /// <param name="sortDirection"></param>
        /// <returns>Sorted list of entities</returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, object>> sortCondition, SortDirection sortDirection);

        /// <summary>
        ///     Gets all entities from database filtered by filter condition
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns>List of filtered entities</returns>
        IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> filterCondition);

        /// <summary>
        ///     Gets all entities from database filtered by filter condition
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns>List of filtered entities</returns>
        Task<IEnumerable<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> filterCondition);

        /// <summary>
        ///     Gets all entities from database filtered by filter condition and sorted by sort condition
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <param name="sortCondition"></param>
        /// <param name="sortDirection"></param>
        /// <returns>Sorted list of filtered entities</returns>
        IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            Expression<Func<TEntity, object>> sortCondition,
            SortDirection sortDirection);

        /// <summary>
        ///     Gets all entities filtered by filter condition for specified page number with defined capacity
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <param name="pageCapacity"></param>
        /// <param name="pageNumber"></param>
        /// <returns>List of filtered entities for specified page number with defined capacity</returns>
        IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            int pageCapacity,
            int pageNumber);

        /// <summary>
        ///     Gets all entities filtered by filter condition and sorted by sort condition for specified page number with defined
        ///     capacity
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <param name="pageCapacity"></param>
        /// <param name="pageNumber"></param>
        /// <param name="sortCondition"></param>
        /// <param name="sortDirection"></param>
        /// <returns>Sorted list of filtered entities for specified page number with defined capacity</returns>
        IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            int pageCapacity,
            int pageNumber,
            Expression<Func<TEntity, object>> sortCondition,
            SortDirection sortDirection);

        // UPDATE

        /// <summary>
        ///     Updates specified entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        ///     Updates list of entities
        /// </summary>
        /// <param name="entities"></param>
        void UpdateRange(IEnumerable<TEntity> entities);

        // DELETE

        /// <summary>
        ///     Deletes entity by Id from database
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        ///     Deletes entity from database
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        ///     Deletes list of entities from database
        /// </summary>
        /// <param name="entities"></param>
        void DeleteRange(IEnumerable<TEntity> entities);

        // OTHER

        /// <summary>
        ///     Gets count of filtered or non-filtered records
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns>Number of filtered/non-filtered records</returns>
        int GetRecordsCount(Expression<Func<TEntity, bool>> filterCondition = null);

        /// <summary>
        ///     Gets pages count
        /// </summary>
        /// <param name="pageCapacity"></param>
        /// <returns>Number of pages</returns>
        int GetPagesCount(int pageCapacity);

        /// <summary>
        ///     Gets pages count filtered by filter condition
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <param name="pageCapacity"></param>
        /// <returns>Number of pages</returns>
        int GetPagesCount(Expression<Func<TEntity, bool>> filterCondition, int pageCapacity);
    }
}