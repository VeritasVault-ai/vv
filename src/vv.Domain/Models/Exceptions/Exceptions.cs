using System;

namespace vv.Domain.Models.Exceptions
{
    public class InvalidMarketDataException : Exception
    {
        public InvalidMarketDataException(string message) : base(message) { }
    }

    public class MarketDataNotFoundException : Exception
    {
        public MarketDataNotFoundException(string assetId)
            : base($"Market data for {assetId} not found") { }
    }
}