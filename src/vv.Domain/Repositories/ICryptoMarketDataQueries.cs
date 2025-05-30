using vv.Domain.Models;
using vv.Domain.Specifications;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace vv.Domain.Repositories
{
    public interface ICryptoMarketDataQueries
    {
        Task<CryptoSpotPriceData> GetSpotPriceAsync(
            string exchange,
            string baseAsset,
            string quoteAsset,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<CryptoSpotPriceData>> GetSpotPricesAsync(
            ISpecification<CryptoSpotPriceData> specification,
            CancellationToken cancellationToken = default);

        Task<CryptoOrderBookData> GetOrderBookAsync(
            string exchange,
            string baseAsset,
            string quoteAsset,
            int depth = 100,
            CancellationToken cancellationToken = default);

        Task<decimal> GetVolumeAsync(
            string exchange,
            string baseAsset,
            string quoteAsset,
            int hours = 24,
            CancellationToken cancellationToken = default);
    }
}