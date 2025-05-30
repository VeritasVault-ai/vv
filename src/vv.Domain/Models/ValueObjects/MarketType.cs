namespace vv.Domain.Models.ValueObjects
{
    public record MarketType
    {
        public static readonly MarketType Spot = new MarketType("spot");
        public static readonly MarketType Futures = new MarketType("futures");
        public static readonly MarketType Perpetual = new MarketType("perpetual");
        public static readonly MarketType Options = new MarketType("options");
        public static readonly MarketType Margin = new MarketType("margin");

        public string Value { get; }

        private MarketType(string value)
        {
            Value = value;
        }

        public static implicit operator string(MarketType marketType) => marketType.Value;

        public static MarketType FromString(string value)
        {
            return value.ToLowerInvariant() switch
            {
                "spot" => Spot,
                "futures" => Futures,
                "perpetual" => Perpetual,
                "perp" => Perpetual,
                "options" => Options,
                "margin" => Margin,
                _ => new MarketType(value.ToLowerInvariant())
            };
        }
    }
}