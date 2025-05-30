using System;

namespace vv.Domain.Models.Exceptions
{
    public class InvalidMarketDataException : Exception
    {
        public InvalidMarketDataException() : base() { }

        public InvalidMarketDataException(string message) : base(message) { }

        public InvalidMarketDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
