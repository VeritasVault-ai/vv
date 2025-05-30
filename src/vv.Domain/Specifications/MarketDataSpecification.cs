using System;
using System.Linq.Expressions;
using vv.Domain.Models;

namespace vv.Domain.Specifications
{
    /// <summary>
    /// Specifications for market data queries
    /// </summary>
    public static class MarketDataSpecification
    {
        /// <summary>
        /// Gets a specification for the latest version of an entity with the given asset ID
        /// </summary>
        public static Expression<Func<T, bool>> LatestVersionByAssetId<T>(string assetId) where T : IMarketDataEntity
        {
            return entity => 
                entity.AssetId == assetId && 
                ((IVersionedEntity)entity).IsLatestVersion;
        }

        /// <summary>
        /// Gets a specification for entities with the given asset ID
        /// </summary>
        public static Expression<Func<T, bool>> ByAssetId<T>(string assetId) where T : IMarketDataEntity
        {
            return entity => entity.AssetId == assetId;
        }

        /// <summary>
        /// Gets a specification for entities with the given asset class
        /// </summary>
        public static Expression<Func<T, bool>> ByAssetClass<T>(string assetClass) where T : IMarketDataEntity
        {
            return entity => entity.AssetClass == assetClass;
        }

        /// <summary>
        /// Gets a specification for entities with the given data type
        /// </summary>
        public static Expression<Func<T, bool>> ByDataType<T>(string dataType) where T : IMarketDataEntity
        {
            return entity => entity.DataType == dataType;
        }

        /// <summary>
        /// Gets a specification for entities with the given region
        /// </summary>
        public static Expression<Func<T, bool>> ByRegion<T>(string region) where T : IMarketDataEntity
        {
            return entity => entity.Region == region;
        }

        /// <summary>
        /// Gets a specification for entities with the given document type
        /// </summary>
        public static Expression<Func<T, bool>> ByDocumentType<T>(string documentType) where T : IMarketDataEntity
        {
            return entity => entity.DocumentType == documentType;
        }

        /// <summary>
        /// Gets a specification for entities with the given as-of date
        /// </summary>
        public static Expression<Func<T, bool>> ByAsOfDate<T>(DateOnly asOfDate) where T : IMarketDataEntity
        {
            return entity => entity.AsOfDate == asOfDate;
        }

        /// <summary>
        /// Gets a specification for entities with an as-of date in the given range
        /// </summary>
        public static Expression<Func<T, bool>> ByAsOfDateRange<T>(DateOnly startDate, DateOnly endDate) where T : IMarketDataEntity
        {
            return entity => entity.AsOfDate >= startDate && entity.AsOfDate <= endDate;
        }

        /// <summary>
        /// Gets a specification for entities with the given tag
        /// </summary>
        public static Expression<Func<T, bool>> ByTag<T>(string tag) where T : IMarketDataEntity
        {
            // Note: This is a simplification. In a real implementation, you would need to
            // use a database-specific approach to query against a collection.
            return entity => entity.Tags.Contains(tag);
        }

        /// <summary>
        /// Gets a specification for entities with the given base version ID
        /// </summary>
        public static Expression<Func<T, bool>> ByBaseVersionId<T>(string? baseVersionId) where T : IMarketDataEntity, IVersionedEntity
        {
            return entity => entity.BaseVersionId == baseVersionId;
        }

        /// <summary>
        /// Gets a specification for entities with the given version
        /// </summary>
        public static Expression<Func<T, bool>> ByVersion<T>(int version) where T : IMarketDataEntity, IVersionedEntity
        {
            return entity => entity.Version == version;
        }

        /// <summary>
        /// Gets a specification for the latest versions of all entities
        /// </summary>
        public static Expression<Func<T, bool>> LatestVersionsOnly<T>() where T : IMarketDataEntity, IVersionedEntity
        {
            return entity => entity.IsLatestVersion;
        }
    }
}
