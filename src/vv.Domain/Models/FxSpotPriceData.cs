using vv.Domain.Constants;

namespace vv.Domain.Models;

public class FxSpotPriceData : BaseMarketData
{
    public decimal Price { get; set; }
    public PriceSide Side { get; set; } = PriceSide.Mid;

    public string Currency { get; set; } = CurrencyCodes.USD;

    /// <summary>
    /// Exchange rate value
    /// </summary>
    public decimal Rate { get; set; }
}