using vv.Data.Repositories;
using vv.Domain.Models;

namespace vv.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IEntityIdGenerator for market data
    /// </summary>
    public class MarketDataIdGenerator<T> : IEntityIdGenerator<T> where T : IMarketDataEntity
    {
        /// <summary>
        /// Generates a unique ID for the given market data entity using a standard format
        /// [dataType]__[assetClass]__[assetId]__[region]__[date]__[documentType]__[version]
        /// </summary>
        public string GenerateId(T entity)
        {
            if (entity == null)
                throw new System.ArgumentNullException(nameof(entity));

            // Use version 1 if version isn't set
            int version = 1;
            if (entity is IVersionedEntity versionedEntity && versionedEntity.Version > 0)
            {
                version = versionedEntity.Version;
            }

            return $"{entity.DataType}__{entity.AssetClass}__{entity.AssetId.ToLowerInvariant()}__{entity.Region}__{entity.AsOfDate:yyyy-MM-dd}__{entity.DocumentType}__{version}";
        }
    }
}