using System;

namespace vv.Domain.Models
{
    public class CryptoPerpetualData : BaseMarketData
    {
        public string Exchange { get; set; }
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public string Symbol { get; set; }
        public decimal MarkPrice { get; set; }
        public decimal IndexPrice { get; set; }
        public decimal FundingRate { get; set; }
        public decimal PredictedFundingRate { get; set; }
        public DateTimeOffset NextFundingTime { get; set; }
        public decimal OpenInterest { get; set; }
        public decimal Volume24H { get; set; }
        public decimal BidPrice { get; set; }
        public decimal AskPrice { get; set; }
        public decimal HighPrice24H { get; set; }
        public decimal LowPrice24H { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        // Domain behavior
        public decimal CalculateBasisPoints() =>
            (MarkPrice - IndexPrice) / IndexPrice * 10000;

        public decimal EstimateDailyFundingCost(decimal positionSize) =>
            positionSize * FundingRate * 3; // 3 funding events per day typically
    }
}