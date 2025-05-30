using System;
using System.Threading;
using System.Threading.Tasks;
using vv.Domain.Models;

namespace vv.Infrastructure.Repositories.Extensions
{
    /// <summary>
    /// Basic extensions for CosmosRepository
    /// </summary>
    public static class CosmosRepositoryExtensions
    {
        /// <summary>
        /// Gets an entity by ID or throws if not found
        /// </summary>
        public static async Task<T> GetByIdOrThrowAsync<T>(
            this CosmosRepository<T> repository,
            string id,
            CancellationToken cancellationToken = default)
            where T : class, IMarketDataEntity
        {
            var entity = await repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new EntityNotFoundException(typeof(T).Name, id);
            return entity;
        }
    }

    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public string EntityId { get; }
        public string EntityType { get; }

        public EntityNotFoundException(string entityType, string entityId)
            : base($"Entity of type {entityType} with ID {entityId} was not found")
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}