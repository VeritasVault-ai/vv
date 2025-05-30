namespace vv.Domain.Constants
{
    public static class CryptoAssets
    {
        // Major cryptocurrencies
        public const string BTC = "BTC";
        public const string ETH = "ETH";
        public const string BNB = "BNB";
        public const string XRP = "XRP";
        public const string ADA = "ADA";
        public const string SOL = "SOL";
        public const string DOT = "DOT";

        // Stablecoins
        public const string USDT = "USDT";
        public const string USDC = "USDC";
        public const string BUSD = "BUSD";
        public const string DAI = "DAI";

        public static readonly string[] MajorCoins = new[]
        {
            BTC, ETH, BNB, XRP, ADA, SOL, DOT
        };

        public static readonly string[] Stablecoins = new[]
        {
            USDT, USDC, BUSD, DAI
        };
    }
}