using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using vv.Data.Repositories;
using vv.Domain.Events;
using vv.Domain.Models;
using vv.Domain.Repositories;
using vv.Infrastructure.Repositories;
using vv.Infrastructure.Repositories.Components;
using System;

namespace vv.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating repository components
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _services;

        public RepositoryFactory(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc/>
        public IMarketDataQueries CreateMarketDataQueries()
        {
            // Get dependencies
            var container = _services.GetRequiredService<Container>();
            var logger = _services.GetRequiredService<ILogger<MarketDataQueries>>();

            // Create cosmos repository for queries
            var cosmosRepo = new CosmosRepository<FxSpotPriceData>(
                container,
                logger,
                null, // No event publisher needed for queries
                entity => entity.AssetId.ToLowerInvariant());

            // Create versioning component
            var versioningComponent = new VersioningComponent<FxSpotPriceData>(
                container,
                logger,
                cosmosRepo,
                cosmosRepo,
                null); // No ID generator needed for queries

            // Create market data queries implementation
            return new MarketDataQueries(
                cosmosRepo,
                versioningComponent,
                logger);
        }

        /// <inheritdoc/>
        public IMarketDataCommands CreateMarketDataCommands()
        {
            // Get dependencies
            var container = _services.GetRequiredService<Container>();
            var logger = _services.GetRequiredService<ILogger<MarketDataCommands>>();
            var idGenerator = _services.GetRequiredService<IEntityIdGenerator<FxSpotPriceData>>();
            var eventPublisher = _services.GetService<IEventPublisher>();

            // Create cosmos repository for commands
            var cosmosRepo = new CosmosRepository<FxSpotPriceData>(
                container,
                logger,
                eventPublisher,
                entity => entity.AssetId.ToLowerInvariant());

            // Create versioning component
            var versioningComponent = new VersioningComponent<FxSpotPriceData>(
                container,
                logger,
                cosmosRepo,
                cosmosRepo,
                idGenerator);

            // Create market data commands implementation
            return new MarketDataCommands(
                cosmosRepo,
                versioningComponent,
                eventPublisher,
                logger);
        }
    }
}