using System;
using vv.Domain.Models.ValueObjects;

namespace vv.Domain.Models
{
    /// <summary>
    /// Represents spot price data for a cryptocurrency trading pair
    /// </summary>
    public class CryptoSpotPriceData : IMarketDataEntity, IVersionedEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the schema version
        /// </summary>
        public string SchemaVersion { get; set; } = "1.0";

        /// <summary>
        /// Gets or sets the asset identifier
        /// </summary>
        public string AssetId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the asset class
        /// </summary>
        public string AssetClass { get; set; } = "cryptocurrency";

        /// <summary>
        /// Gets or sets the data type
        /// </summary>
        public string DataType { get; set; } = "spot_price";

        /// <summary>
        /// Gets or sets the geographical region
        /// </summary>
        public string Region { get; set; } = "global";

        /// <summary>
        /// Gets the collection of tags
        /// </summary>
        public IReadOnlyList<string> Tags => _tags;
        private readonly List<string> _tags = new();

        /// <summary>
        /// Gets or sets the document type
        /// </summary>
        public string DocumentType { get; set; } = "market_data";

        /// <summary>
        /// Gets the timestamp when this entity was created
        /// </summary>
        public DateTimeOffset CreateTimestamp { get; private set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets or sets the business date
        /// </summary>
        public DateOnly AsOfDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        /// <summary>
        /// Gets or sets the time component
        /// </summary>
        public TimeOnly? AsOfTime { get; set; } = TimeOnly.FromDateTime(DateTime.UtcNow);

        /// <summary>
        /// Gets or sets the version number
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// Gets or sets the base version ID
        /// </summary>
        public string? BaseVersionId { get; set; }

        /// <summary>
        /// Gets or sets whether this entity is the latest version
        /// </summary>
        public bool IsLatestVersion { get; set; } = true;

        /// <summary>
        /// Gets or sets the base asset symbol
        /// </summary>
        public string BaseAsset { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quote asset symbol
        /// </summary>
        public string QuoteAsset { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exchange name
        /// </summary>
        public string Exchange { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last traded price
        /// </summary>
        public decimal LastPrice { get; set; }

        /// <summary>
        /// Gets or sets the 24-hour high price
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// Gets or sets the 24-hour low price
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// Gets or sets the 24-hour volume in base asset
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the 24-hour volume in quote asset
        /// </summary>
        public decimal QuoteVolume { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the price data
        /// </summary>
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets or sets the bid price
        /// </summary>
        public decimal BidPrice { get; set; }

        /// <summary>
        /// Gets or sets the ask price
        /// </summary>
        public decimal AskPrice { get; set; }

        /// <summary>
        /// Gets or sets the 24-hour price change percentage
        /// </summary>
        public decimal PriceChangePercent { get; set; }

        /// <summary>
        /// Gets or sets the number of trades in the last 24 hours
        /// </summary>
        public int NumberOfTrades { get; set; }

        /// <summary>
        /// Gets or sets whether this data point is considered an anomaly
        /// </summary>
        public bool IsAnomalous { get; set; }

        /// <summary>
        /// Gets or sets the data source
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional metadata
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Adds a tag to the entity
        /// </summary>
        public void AddTag(string tag)
        {
            if (!string.IsNullOrEmpty(tag) && !_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
        }

        /// <summary>
        /// Removes a tag from the entity
        /// </summary>
        public void RemoveTag(string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                _tags.Remove(tag);
            }
        }
    }
}
