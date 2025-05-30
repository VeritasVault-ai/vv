using vv.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace vv.Domain.Repositories.Components
{
    /// <summary>
    /// Interface for base repository operations
    /// </summary>
    public interface IRepository<T> where T : IMarketDataEntity
    {
        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an entity exists
        /// </summary>
        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync(bool includeSoftDeleted = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Queries entities using a predicate
        /// </summary>
        Task<IEnumerable<T>> QueryAsync(
            Expression<Func<T, bool>> predicate,
            bool includeSoftDeleted = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts entities matching a predicate
        /// </summary>
        Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets entities with paging
        /// </summary>
        Task<(IEnumerable<T> Items, string? ContinuationToken)> GetPagedAsync(
            int pageSize,
            string? continuationToken = null,
            bool includeSoftDeleted = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new entity
        /// </summary>
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task<T> UpdateAsync(T entity, string? etag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates or updates an entity
        /// </summary>
        Task<T> UpsertAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts multiple entities
        /// </summary>
        Task<int> BulkInsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        Task<bool> DeleteAsync(string id, bool soft = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Permanently removes soft-deleted entities
        /// </summary>
        Task<int> PurgeSoftDeletedAsync(CancellationToken cancellationToken = default);
    }
}