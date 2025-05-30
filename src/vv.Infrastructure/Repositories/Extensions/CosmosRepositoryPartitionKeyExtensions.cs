using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using vv.Domain.Models;

namespace vv.Infrastructure.Repositories.Extensions
{
    /// <summary>
    /// Advanced partition key resolution extensions for CosmosRepository
    /// </summary>
    public static class CosmosRepositoryPartitionKeyExtensions
    {
        /// <summary>
        /// Gets the partition key for a specific entity ID.
        /// This is a fallback method when we only have the ID but not the full entity.
        /// </summary>
        public static async Task<PartitionKey> GetPartitionKeyForIdAsync<T>(
            this CosmosRepository<T> repository,
            Container container,
            ILogger logger,
            string id,
            Func<T, string> partitionKeyResolver,
            CancellationToken cancellationToken = default)
            where T : class, IMarketDataEntity
        {
            try
            {
                // First attempt: If ID format contains partition info, extract it
                string possiblePartitionKey = ExtractPossiblePartitionKeyFromId(id);
                if (!string.IsNullOrEmpty(possiblePartitionKey))
                {
                    return new PartitionKey(possiblePartitionKey);
                }

                // Try known partition key values
                var likelyPartitionKeys = GetLikelyPartitionKeysForId(id);

                foreach (var potentialKey in likelyPartitionKeys)
                {
                    try
                    {
                        var queryOptions = new QueryRequestOptions
                        {
                            PartitionKey = new PartitionKey(potentialKey)
                        };

                        var queryDefinition = new QueryDefinition(
                            "SELECT * FROM c WHERE c.id = @id")
                            .WithParameter("@id", id);

                        var iterator = container.GetItemQueryIterator<T>(queryDefinition, requestOptions: queryOptions);
                        if (iterator == null)
                        {
                            logger.LogWarning("Query iterator is null for ID {Id} with potential key {Key}", id, potentialKey);
                            continue;
                        }

                        while (iterator.HasMoreResults)
                        {
                            var response = await iterator.ReadNextAsync(cancellationToken);
                            if (response != null && response.Count > 0)
                            {
                                var entity = response.FirstOrDefault();
                                if (entity != null)
                                {
                                    return new PartitionKey(partitionKeyResolver(entity));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Error trying partition key {Key} for ID {Id}", potentialKey, id);
                        // Continue to the next potential key
                    }
                }

                // If we can't find the entity, fall back to using the ID
                logger.LogWarning("Could not determine partition key for entity with ID {Id}, falling back to ID", id);
                return new PartitionKey(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error determining partition key for entity with ID {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Extract a possible partition key from the ID if the ID follows a known pattern
        /// </summary>
        public static string ExtractPossiblePartitionKeyFromId(string id)
        {
            // For market data entities, the partition key is usually in the ID format
            // Example: "price.spot__fx__eurusd__global__2023-05-14__official__1"
            // where "eurusd" is the assetId and should be the partition key
            if (id.Contains("__"))
            {
                var parts = id.Split("__");
                if (parts.Length >= 3)
                {
                    // AssetId is typically the third part in this format
                    return parts[2];
                }
            }

            // For other ID formats
            if (id.Contains('_'))
            {
                return id.Split('_').FirstOrDefault() ?? string.Empty;
            }

            if (id.Contains('.'))
            {
                return id.Split('.').FirstOrDefault() ?? string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get a list of likely partition keys to check for an entity with the given ID
        /// </summary>
        public static List<string> GetLikelyPartitionKeysForId(string id)
        {
            // This is domain-specific logic for market data entities
            // The partition key is usually the asset ID, which might be part of the ID

            var possibleKeys = new List<string>
            {
                id,  // The ID itself
            };

            // For market data entities
            if (id.Contains("__"))
            {
                var parts = id.Split("__");
                if (parts.Length >= 3)
                {
                    possibleKeys.Add(parts[2]);  // assetId is typically the third part
                }
            }

            // Other potential patterns
            possibleKeys.AddRange(new[]
            {
                id.Split('_').FirstOrDefault() ?? string.Empty,  // First part of ID if delimited
                id.Split('.').FirstOrDefault() ?? string.Empty   // First part if using dot notation
            });

            return possibleKeys.Where(k => !string.IsNullOrEmpty(k)).Distinct().ToList();
        }
    }
}