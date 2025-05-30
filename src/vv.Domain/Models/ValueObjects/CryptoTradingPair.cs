using System;

namespace vv.Domain.Models.ValueObjects
{
    public record CryptoTradingPair
    {
        public string BaseAsset { get; }
        public string QuoteAsset { get; }
        public string Exchange { get; }

        public CryptoTradingPair(string baseAsset, string quoteAsset, string exchange)
        {
            if (string.IsNullOrEmpty(baseAsset))
                throw new ArgumentException("Base asset cannot be empty", nameof(baseAsset));

            if (string.IsNullOrEmpty(quoteAsset))
                throw new ArgumentException("Quote asset cannot be empty", nameof(quoteAsset));

            if (string.IsNullOrEmpty(exchange))
                throw new ArgumentException("Exchange cannot be empty", nameof(exchange));

            BaseAsset = baseAsset.ToUpperInvariant();
            QuoteAsset = quoteAsset.ToUpperInvariant();
            Exchange = exchange.ToLowerInvariant();
        }

        // Different exchanges use different formats
        public string GetExchangeSymbol()
        {
            return Exchange switch
            {
                "binance" => $"{BaseAsset}{QuoteAsset}",
                "coinbase" => $"{BaseAsset}-{QuoteAsset}",
                "kraken" => BaseAsset == "BTC" ? $"X{BaseAsset}Z{QuoteAsset}" : $"{BaseAsset}{QuoteAsset}",
                "bitfinex" => $"t{BaseAsset}{QuoteAsset}",
                _ => $"{BaseAsset}{QuoteAsset}"
            };
        }

        // Globally unique identifier
        public string UniqueId => $"{Exchange}:{BaseAsset}/{QuoteAsset}".ToLowerInvariant();

        // Standard format for display
        public string DisplayPair => $"{BaseAsset}/{QuoteAsset}";
    }
}