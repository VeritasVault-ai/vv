using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using vv.Domain.Models;

namespace vv.Domain.Repositories
{
    public interface IMarketDataRepository
    {
        Task<FxSpotPriceData> GetLatestMarketDataAsync(
            string assetId,
            string assetClass,
            string dataType,
            string exchange,
            DateOnly asOfDate,
            string currency);

        Task<IEnumerable<FxSpotPriceData>> QueryAsync(Func<FxSpotPriceData, bool> predicate);

        Task<FxSpotPriceData> CreateMarketDataAsync(FxSpotPriceData data);

        Task<IEnumerable<FxSpotPriceData>> QueryByRangeAsync(
            string dataType,
            string assetClass,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken = default);
    }
}
