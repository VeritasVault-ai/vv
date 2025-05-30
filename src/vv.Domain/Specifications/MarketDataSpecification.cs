using vv.Domain.Extensions;
using vv.Domain.Models;
using System;
using System.Linq.Expressions;

namespace vv.Domain.Specifications
{
    /// <summary>
    /// Specification for market data queries
    /// </summary>
    public class MarketDataSpecification : ISpecification<FxSpotPriceData>
    {
        private Expression<Func<FxSpotPriceData, bool>> _criteria = x => true;

        /// <summary>
        /// Convert to expression
        /// </summary>
        public Expression<Func<FxSpotPriceData, bool>> ToExpression()
        {
            return _criteria;
        }

        /// <summary>
        /// Check if entity satisfies this specification
        /// </summary>
        public bool IsSatisfiedBy(FxSpotPriceData entity)
        {
            var predicate = _criteria.Compile();
            return predicate(entity);
        }

        /// <summary>
        /// Filter by data type
        /// </summary>
        public MarketDataSpecification WithDataType(string dataType)
        {
            _criteria = _criteria.And(x => x.DataType == dataType);
            return this;
        }

        /// <summary>
        /// Filter by asset class
        /// </summary>
        public MarketDataSpecification WithAssetClass(string assetClass)
        {
            _criteria = _criteria.And(x => x.AssetClass == assetClass);
            return this;
        }

        /// <summary>
        /// Filter by asset ID
        /// </summary>
        public MarketDataSpecification WithAssetId(string assetId)
        {
            _criteria = _criteria.And(x => x.AssetId == assetId.ToLowerInvariant());
            return this;
        }

        /// <summary>
        /// Filter by region
        /// </summary>
        public MarketDataSpecification WithRegion(string region)
        {
            _criteria = _criteria.And(x => x.Region == region);
            return this;
        }

        /// <summary>
        /// Filter by as-of date
        /// </summary>
        public MarketDataSpecification WithAsOfDate(DateOnly asOfDate)
        {
            _criteria = _criteria.And(x => x.AsOfDate == asOfDate);
            return this;
        }

        /// <summary>
        /// Filter by document type
        /// </summary>
        public MarketDataSpecification WithDocumentType(string documentType)
        {
            _criteria = _criteria.And(x => x.DocumentType == documentType);
            return this;
        }

        /// <summary>
        /// Filter by from date
        /// </summary>
        public MarketDataSpecification WithFromDate(DateOnly fromDate)
        {
            _criteria = _criteria.And(x => x.AsOfDate >= fromDate);
            return this;
        }

        /// <summary>
        /// Filter by to date
        /// </summary>
        public MarketDataSpecification WithToDate(DateOnly toDate)
        {
            _criteria = _criteria.And(x => x.AsOfDate <= toDate);
            return this;
        }

        /// <summary>
        /// Filter by exchange name
        /// </summary>
        public MarketDataSpecification WithExchange(string exchangeName)
        {
            _criteria = _criteria.And(x => x.Exchange == exchangeName);
            return this;
        }

        /// <summary>
        /// Filter by market type (spot, futures, perpetual, options)
        /// </summary>
        public MarketDataSpecification WithMarketType(string marketType)
        {
            _criteria = _criteria.And(x => x.MarketType == marketType);
            return this;
        }

        /// <summary>
        /// Filter by base asset (e.g., "BTC" in BTC/USDT)
        /// </summary>
        public MarketDataSpecification WithBaseAsset(string baseAsset)
        {
            _criteria = _criteria.And(x => x.BaseAsset == baseAsset.ToUpperInvariant());
            return this;
        }

        /// <summary>
        /// Filter by quote asset (e.g., "USDT" in BTC/USDT)
        /// </summary>
        public MarketDataSpecification WithQuoteAsset(string quoteAsset)
        {
            _criteria = _criteria.And(x => x.QuoteAsset == quoteAsset.ToUpperInvariant());
            return this;
        }

        /// <summary>
        /// Create a specification for a crypto trading pair on a specific exchange
        /// </summary>
        public static MarketDataSpecification ForCryptoTradingPair(string baseAsset, string quoteAsset, string exchange = null)
        {
            var spec = new MarketDataSpecification()
                .WithDataType("price.spot")
                .WithAssetClass("crypto")
                .WithBaseAsset(baseAsset)
                .WithQuoteAsset(quoteAsset);

            if (!string.IsNullOrEmpty(exchange))
            {
                spec.WithExchange(exchange);
            }

            return spec;
        }

        /// <summary>
        /// Create a specification for all trading pairs of a base asset (e.g., all BTC pairs)
        /// </summary>
        public static MarketDataSpecification ForBaseAsset(string baseAsset, string exchange = null)
        {
            var spec = new MarketDataSpecification()
                .WithDataType("price.spot")
                .WithAssetClass("crypto")
                .WithBaseAsset(baseAsset);

            if (!string.IsNullOrEmpty(exchange))
            {
                spec.WithExchange(exchange);
            }

            return spec;
        }

        /// <summary>
        /// Create a specification for a specific exchange's market data
        /// </summary>
        public static MarketDataSpecification ForExchange(string exchange, string marketType = "spot")
        {
            return new MarketDataSpecification()
                .WithAssetClass("crypto")
                .WithExchange(exchange)
                .WithMarketType(marketType);
        }

        /// <summary>
        /// Create a specification for stablecoin pairs (USDT, USDC, DAI, etc.)
        /// </summary>
        public static MarketDataSpecification ForStablecoinPairs(string baseAsset, string[] stablecoins = null)
        {
            stablecoins ??= new[] { "USDT", "USDC", "BUSD", "DAI", "UST" };

            var spec = new MarketDataSpecification()
                .WithDataType("price.spot")
                .WithAssetClass("crypto")
                .WithBaseAsset(baseAsset);

            // Create a combined predicate for any of the stablecoins
            Expression<Func<FxSpotPriceData, bool>> stablecoinPredicate = null;
            foreach (var stablecoin in stablecoins)
            {
                var coin = stablecoin.ToUpperInvariant();
                var predicate = (Expression<Func<FxSpotPriceData, bool>>)(x => x.QuoteAsset == coin);

                stablecoinPredicate = stablecoinPredicate == null
                    ? predicate
                    : stablecoinPredicate.Or(predicate);
            }

            if (stablecoinPredicate != null)
            {
                spec._criteria = spec._criteria.And(stablecoinPredicate);
            }

            return spec;
        }
    }
}