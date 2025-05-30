using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using vv.Domain.Models;

namespace vv.Domain.Services
{
    /// <summary>
    /// Service for managing cryptocurrency market data
    /// </summary>
    public interface ICryptoMarketDataService
    {
        /// <summary>
        /// Gets the latest price data for a cryptocurrency trading pair
        /// </summary>
        /// <param name="baseAsset">Base asset symbol</param>
        /// <param name="quoteAsset">Quote asset symbol</param>
        /// <param name="exchange">Exchange name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The latest price data, or null if not found</returns>
        Task<CryptoSpotPriceData?> GetLatestPriceDataAsync(
            string baseAsset,
            string quoteAsset,
            string exchange,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical price data for a cryptocurrency trading pair
        /// </summary>
        /// <param name="baseAsset">Base asset symbol</param>
        /// <param name="quoteAsset">Quote asset symbol</param>
        /// <param name="exchange">Exchange name</param>
        /// <param name="startDate">Start date for historical data</param>
        /// <param name="endDate">End date for historical data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of historical price data points</returns>
        Task<IEnumerable<CryptoSpotPriceData?>> GetHistoricalPriceDataAsync(
            string baseAsset,
            string quoteAsset,
            string exchange,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves cryptocurrency price data
        /// </summary>
        /// <param name="priceData">Price data to save</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The saved price data</returns>
        Task<CryptoSpotPriceData> SavePriceDataAsync(
            CryptoSpotPriceData priceData,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Detects anomalies in cryptocurrency price data
        /// </summary>
        /// <param name="baseAsset">Base asset symbol</param>
        /// <param name="quoteAsset">Quote asset symbol</param>
        /// <param name="exchange">Exchange name</param>
        /// <param name="lookbackDays">Number of days to look back for anomaly detection</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of anomalous price data points</returns>
        Task<IEnumerable<CryptoSpotPriceData>> DetectAnomaliesAsync(
            string baseAsset,
            string quoteAsset,
            string exchange,
            int lookbackDays = 30,
            CancellationToken cancellationToken = default);
    }
}
