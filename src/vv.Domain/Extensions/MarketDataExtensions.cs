using vv.Domain.Models;
using System;

namespace vv.Domain.Extensions
{
    public static class MarketDataExtensions
    {
        public static CryptoTradingPair ToTradingPair(this CryptoSpotPriceData data)
        {
            return new CryptoTradingPair(data.BaseAsset, data.QuoteAsset, data.Exchange);
        }

        public static decimal CalculateImpliedVolatility(this CryptoSpotPriceData data, CryptoSpotPriceData previousData)
        {
            if (previousData == null || previousData.LastPrice == 0)
                return 0;

            var priceReturn = Math.Log((double)(data.LastPrice / previousData.LastPrice));
            return (decimal)Math.Abs(priceReturn);
        }

        public static bool IsPriceAnomalous(this CryptoSpotPriceData current, CryptoSpotPriceData previous, decimal thresholdPercent = 5.0m)
        {
            if (previous == null || previous.LastPrice == 0)
                return false;

            var percentChange = Math.Abs((current.LastPrice - previous.LastPrice) / previous.LastPrice * 100);
            return percentChange > thresholdPercent;
        }
    }
}