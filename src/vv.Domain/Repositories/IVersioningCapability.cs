using vv.Domain.Models;
using vv.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace vv.Domain.Repositories.Components
{
    /// <summary>
    /// Interface for versioning capabilities
    /// </summary>
    public interface IVersioningCapability<T> where T : IMarketDataEntity, IVersionedEntity
    {
        /// <summary>
        /// Gets the next version number for entities matching the specification
        /// </summary>
        Task<int> GetNextVersionAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the next version number for entities matching the predicate
        /// </summary>
        Task<int> GetNextVersionAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the latest version of an entity matching the specification
        /// </summary>
        Task<(T? Result, string? ETag)> GetByLatestVersionAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the latest version of an entity matching the predicate
        /// </summary>
        Task<(T? Result, string? ETag)> GetByLatestVersionAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a specific version of an entity matching the specification
        /// </summary>
        Task<(T? Result, string? ETag)> GetBySpecifiedVersionAsync(
            ISpecification<T> specification,
            int version,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a specific version of an entity matching the predicate
        /// </summary>
        Task<(T? Result, string? ETag)> GetBySpecifiedVersionAsync(
            Expression<Func<T, bool>> predicate,
            int version,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all versions of an entity with the given base ID
        /// </summary>
        Task<IEnumerable<T>> GetAllVersionsAsync(
            string baseId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves an entity with versioning, using a specification to identify related entities
        /// </summary>
        Task<T> SaveVersionedEntityAsync(
            T entity,
            ISpecification<T> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves an entity with versioning, using a predicate to identify related entities
        /// </summary>
        Task<T> SaveVersionedEntityAsync(
            T entity,
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);
    }
}