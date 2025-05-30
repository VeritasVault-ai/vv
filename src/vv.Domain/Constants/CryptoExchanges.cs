namespace vv.Domain.Constants
{
    public static class CryptoExchanges
    {
        public const string Binance = "binance";
        public const string Coinbase = "coinbase";
        public const string Kraken = "kraken";
        public const string Bitfinex = "bitfinex";
        public const string FTX = "ftx";
        public const string Huobi = "huobi";
        public const string OKX = "okx";
        public const string Bybit = "bybit";

        public static readonly string[] MajorExchanges = new[]
        {
            Binance, Coinbase, Kraken, Bitfinex, Huobi, OKX, Bybit
        };
    }
}