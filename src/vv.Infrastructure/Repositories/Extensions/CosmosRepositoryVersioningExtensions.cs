using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using vv.Domain.Models;

namespace vv.Infrastructure.Repositories.Extensions
{
    /// <summary>
    /// Extension methods for versioning functionality in CosmosRepository
    /// </summary>
    public static class CosmosRepositoryVersioningExtensions
    {
        /// <summary>
        /// Gets the next available version number for a market data entity with the specified criteria.
        /// </summary>
        /// <typeparam name="T">The type of market data entity</typeparam>
        /// <param name="repo">The repository instance</param>
        /// <param name="dataType">The data type identifier</param>
        /// <param name="assetClass">The asset class</param>
        /// <param name="assetId">The asset identifier</param>
        /// <param name="region">The region code</param>
        /// <param name="asOfDate">The as-of date</param>
        /// <param name="documentType">The document type</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The next version number (starting from 1 for new entities)</returns>
        public static async Task<int> GetNextVersionAsync<T>(
            this CosmosRepository<T> repo,
            string dataType,
            string assetClass,
            string assetId,
            string region,
            DateOnly asOfDate,
            string documentType,
            CancellationToken cancellationToken = default)
            where T : class, IMarketDataEntity
        {
            var container = repo.GetContainer();
            var query = container.GetItemLinqQueryable<T>(allowSynchronousQueryExecution: false)
                .Where(e =>
                    e.AssetId == assetId &&
                    e.AssetClass == assetClass &&
                    e.Region == region &&
                    e.DataType == dataType &&
                    e.DocumentType == documentType &&
                    e.AsOfDate == asOfDate)
                .OrderByDescending(e => e.Version)
                .Take(1)
                .ToFeedIterator();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync(cancellationToken);
                var latest = response.FirstOrDefault();
                if (latest != null)
                {
                    // Handle the nullable Version property properly
                    int currentVersion = latest.Version ?? 0;
                    return currentVersion + 1;
                }
            }
            return 1;
        }

        /// <summary>
        /// Updates a market data entity with optimistic concurrency control to prevent race conditions
        /// </summary>
        /// <typeparam name="T">The type of market data entity</typeparam>
        /// <param name="repo">The repository instance</param>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The updated entity</returns>
        /// <exception cref="ConcurrencyException">Thrown when a concurrency conflict occurs</exception>
        public static async Task<T> UpdateWithOptimisticConcurrencyAsync<T>(
            this CosmosRepository<T> repo,
            T entity,
            CancellationToken cancellationToken = default)
            where T : class, IMarketDataEntity
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                // Cosmos DB uses ETags for optimistic concurrency control
                var container = repo.GetContainer();

                // The ETag for the entity should be stored in the entity if it implements IETaggable
                string? etag = (entity as IETaggable)?.ETag;

                ItemRequestOptions? options = null;
                if (!string.IsNullOrEmpty(etag))
                {
                    options = new ItemRequestOptions
                    {
                        IfMatchEtag = etag
                    };
                }

                // Get the partition key from the repository
                PartitionKey partitionKey = repo.GetPartitionKey(entity);

                var response = await container.ReplaceItemAsync(
                    entity,
                    entity.Id,
                    partitionKey,
                    options,
                    cancellationToken);

                // If entity implements IETaggable, update the ETag
                if (entity is IETaggable etaggable)
                {
                    etaggable.ETag = response.ETag;
                }

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
            {
                // Handle the concurrency conflict
                throw new ConcurrencyException(
                    "Another process has modified this entity. Please reload and try again.",
                    ex);
            }
        }
    }

    /// <summary>
    /// Interface for entities that support ETag-based concurrency control
    /// </summary>
    public interface IETaggable
    {
        /// <summary>
        /// Gets or sets the ETag value for optimistic concurrency control
        /// </summary>
        string? ETag { get; set; }
    }

    /// <summary>
    /// Exception thrown when a concurrency conflict is detected
    /// </summary>
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message) : base(message) { }
        public ConcurrencyException(string message, Exception innerException) : base(message, innerException) { }
    }
}