using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using vv.Domain.Events;
using vv.Domain.Models;
using vv.Domain.Repositories;
using vv.Infrastructure.Configuration;

namespace vv.Infrastructure.Repositories
{
    /// <summary>
    /// Factory for creating Cosmos DB repositories
    /// </summary>
    public class CosmosRepositoryFactory : IRepositoryFactory
    {
        private readonly CosmosClient _cosmosClient;
        private readonly ILoggerFactory _loggerFactory;
        private readonly CosmosDbOptions _options;
        private readonly IEventPublisher _eventPublisher;

        public CosmosRepositoryFactory(
            CosmosClient cosmosClient,
            ILoggerFactory loggerFactory,
            IOptions<CosmosDbOptions> options,
            IEventPublisher eventPublisher)
        {
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        /// <inheritdoc/>
        public TRepository CreateRepository<T, TRepository>()
            where T : class, IMarketDataEntity
            where TRepository : IRepository<T>
        {
            // Get container based on entity type
            var containerName = GetContainerNameForType(typeof(T));
            var container = _cosmosClient.GetContainer(_options.DatabaseName, containerName);

            // Create appropriate logger
            var logger = _loggerFactory.CreateLogger<CosmosRepository<T>>();

            // Create ID generator if needed
            IEntityIdGenerator<T> idGenerator = null;
            if (typeof(T) == typeof(FxSpotPriceData))
            {
                idGenerator = new MarketDataIdGenerator() as IEntityIdGenerator<T>;
            }

            // Instantiate the repository based on the type
            if (typeof(TRepository) == typeof(IMarketDataRepository) && typeof(T) == typeof(FxSpotPriceData))
            {
                return new MarketDataRepository(
                    container,
                    logger as ILogger<CosmosRepository<FxSpotPriceData>>,
                    idGenerator as IEntityIdGenerator<FxSpotPriceData>,
                    _eventPublisher) as TRepository;
            }

            // Default repository if no specific type matches
            if (typeof(T).GetInterface(nameof(IVersionedEntity)) != null)
            {
                return new VersionedCosmosRepository<T>(
                    container,
                    logger,
                    idGenerator,
                    _eventPublisher) as TRepository;
            }

            return new CosmosRepository<T>(container, logger, _eventPublisher) as TRepository;
        }

        /// <inheritdoc/>
        public IMarketDataRepository CreateMarketDataRepository()
        {
            return CreateRepository<FxSpotPriceData, IMarketDataRepository>();
        }

        /// <summary>
        /// Gets the container name for an entity type
        /// </summary>
        private string GetContainerNameForType(Type entityType)
        {
            // Use type name as container name by default
            var containerName = entityType.Name;

            // Override for specific types if needed
            if (entityType == typeof(FxSpotPriceData))
            {
                containerName = _options.MarketDataContainerName;
            }

            return containerName;
        }
    }
}