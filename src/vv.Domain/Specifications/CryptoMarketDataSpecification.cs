using vv.Domain.Extensions;
using vv.Domain.Models;
using System;
using System.Linq.Expressions;

namespace vv.Domain.Specifications
{
    public class CryptoMarketDataSpecification : ISpecification<CryptoSpotPriceData>
    {
        private Expression<Func<CryptoSpotPriceData, bool>> _criteria = x => true;

        public Expression<Func<CryptoSpotPriceData, bool>> ToExpression() => _criteria;

        public bool IsSatisfiedBy(CryptoSpotPriceData entity)
        {
            var predicate = _criteria.Compile();
            return predicate(entity);
        }

        public CryptoMarketDataSpecification WithExchange(string exchange)
        {
            _criteria = _criteria.And(x => x.Exchange == exchange.ToLowerInvariant());
            return this;
        }

        public CryptoMarketDataSpecification WithBaseAsset(string baseAsset)
        {
            _criteria = _criteria.And(x => x.BaseAsset == baseAsset.ToUpperInvariant());
            return this;
        }

        public CryptoMarketDataSpecification WithQuoteAsset(string quoteAsset)
        {
            _criteria = _criteria.And(x => x.QuoteAsset == quoteAsset.ToUpperInvariant());
            return this;
        }

        public CryptoMarketDataSpecification WithSymbol(string symbol)
        {
            _criteria = _criteria.And(x => x.AssetId.Contains(symbol, StringComparison.OrdinalIgnoreCase));
            return this;
        }

        public CryptoMarketDataSpecification WithMinVolume(decimal minVolume)
        {
            _criteria = _criteria.And(x => x.Volume >= minVolume);
            return this;
        }

        public CryptoMarketDataSpecification WithFromDate(DateOnly fromDate)
        {
            _criteria = _criteria.And(x => x.AsOfDate >= fromDate);
            return this;
        }

        public CryptoMarketDataSpecification WithToDate(DateOnly toDate)
        {
            _criteria = _criteria.And(x => x.AsOfDate <= toDate);
            return this;
        }

        // Factory methods
        public static CryptoMarketDataSpecification ForTradingPair(string baseAsset, string quoteAsset)
        {
            return new CryptoMarketDataSpecification()
                .WithBaseAsset(baseAsset)
                .WithQuoteAsset(quoteAsset);
        }

        public static CryptoMarketDataSpecification ForExchange(string exchange)
        {
            return new CryptoMarketDataSpecification()
                .WithExchange(exchange);
        }

        public static CryptoMarketDataSpecification ForTopVolume(int count = 100)
        {
            // Note: This won't work directly with basic expressions
            // TODO: need to implement special handling in the repository
            return new CryptoMarketDataSpecification();
        }
    }
}
