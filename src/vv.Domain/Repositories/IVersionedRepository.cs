using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using vv.Domain.Models;
using vv.Domain.Repositories.Components;

namespace vv.Domain.Repositories
{
    /// <summary>
    /// Repository interface for versioned entities
    /// </summary>
    public interface IVersionedRepository<T> : IRepository<T> where T : IMarketDataEntity
    {
        /// <summary>
        /// Gets the next available version number for an entity
        /// </summary>
        Task<int> GetNextVersionAsync(
            Expression<Func<T, bool>> keyPredicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the entity with the latest version that matches the predicate
        /// </summary>
        Task<(T? Result, string? ETag)> GetByLatestVersionAsync(
            Expression<Func<T, bool>> keyPredicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an entity with a specific version
        /// </summary>
        Task<(T? Result, string? ETag)> GetBySpecifiedVersionAsync(
            Expression<Func<T, bool>> keyPredicate,
            int version,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all versions of an entity with the specified base ID
        /// </summary>
        Task<IEnumerable<T>> GetAllVersionsAsync(
            string baseId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves a versioned entity
        /// </summary>
        Task<T> SaveVersionedEntityAsync(
            T entity,
            Expression<Func<T, bool>> keyPredicate,
            CancellationToken cancellationToken = default);
    }
}
