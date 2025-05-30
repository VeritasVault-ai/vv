using System;

namespace vv.Domain.Models.ValueObjects
{
    public record CurrencyPair
    {
        public string BaseCurrency { get; }
        public string QuoteCurrency { get; }

        public CurrencyPair(string baseCurrency, string quoteCurrency)
        {
            if (string.IsNullOrEmpty(baseCurrency))
                throw new ArgumentException("Base currency cannot be empty", nameof(baseCurrency));

            if (string.IsNullOrEmpty(quoteCurrency))
                throw new ArgumentException("Quote currency cannot be empty", nameof(quoteCurrency));

            BaseCurrency = baseCurrency.ToUpperInvariant();
            QuoteCurrency = quoteCurrency.ToUpperInvariant();
        }

        public string AssetId => $"{BaseCurrency}{QuoteCurrency}".ToLowerInvariant();

        public static CurrencyPair FromAssetId(string assetId)
        {
            if (assetId?.Length != 6)
                throw new ArgumentException("Asset ID must be 6 characters (e.g., 'eurusd')", nameof(assetId));

            return new CurrencyPair(
                assetId.Substring(0, 3),
                assetId.Substring(3, 3)
            );
        }
    }
}