using System;
using System.Collections.Generic;

namespace vv.Domain.Models
{
    public class CryptoOrderBookData : BaseMarketData
    {
        public string Exchange { get; set; }
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public string Symbol { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public List<OrderBookLevel> Bids { get; set; } = new List<OrderBookLevel>();
        public List<OrderBookLevel> Asks { get; set; } = new List<OrderBookLevel>();
        public long LastUpdateId { get; set; }

        // Domain behavior
        public decimal CalculateSpread() =>
            Asks.Count > 0 && Bids.Count > 0 ?
            Asks[0].Price - Bids[0].Price : 0;

        public decimal CalculateMidPrice() =>
            Asks.Count > 0 && Bids.Count > 0 ?
            (Asks[0].Price + Bids[0].Price) / 2 : 0;

        public decimal CalculateBidDepth(int levels) =>
            Bids.Take(levels).Sum(level => level.Price * level.Quantity);

        public decimal CalculateAskDepth(int levels) =>
            Asks.Take(levels).Sum(level => level.Price * level.Quantity);
    }

    public class OrderBookLevel
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}