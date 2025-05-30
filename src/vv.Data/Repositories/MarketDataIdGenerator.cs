using vv.Domain.Models;

namespace vv.Data.Repositories
{
    /// <summary>
    /// Generic implementation of ID generator for market data
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

            return $"{entity.DataType}__{entity.AssetClass}__{entity.AssetId.ToLowerInvariant()}__{entity.Region}__{entity.AsOfDate:yyyy-MM-dd}__{entity.DocumentType}__{entity.Version}";
        }
    }
}