using System;

namespace vv.Domain.Models
{
    public class CryptoSpotPriceData : BaseMarketData
    {
        public string Exchange { get; set; }
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public string Symbol { get; set; }  // Native exchange symbol (e.g., "BTCUSDT")
        public decimal LastPrice { get; set; }
        public decimal BidPrice { get; set; }
        public decimal AskPrice { get; set; }
        public decimal Volume24H { get; set; }
        public decimal QuoteVolume24H { get; set; }
        public decimal PriceChangePercent24H { get; set; }
        public decimal HighPrice24H { get; set; }
        public decimal LowPrice24H { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        // Domain behavior
        public decimal CalculateMidPrice() => (BidPrice + AskPrice) / 2;
        public decimal CalculateSpread() => AskPrice - BidPrice;
        public decimal CalculateSpreadPercent() => (AskPrice - BidPrice) / AskPrice * 100;

        // Domain validation
        public void Validate()
        {
            if (string.IsNullOrEmpty(Exchange))
                throw new ArgumentException("Exchange is required");

            if (string.IsNullOrEmpty(BaseAsset) || string.IsNullOrEmpty(QuoteAsset))
                throw new ArgumentException("Base and quote assets are required");

            if (BidPrice <= 0 || AskPrice <= 0)
                throw new ArgumentException("Prices must be positive");

            if (BidPrice > AskPrice)
                throw new ArgumentException("Bid price cannot be higher than ask price");
        }
    }
}