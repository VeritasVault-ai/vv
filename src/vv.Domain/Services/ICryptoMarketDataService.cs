using vv.Domain.Models;
using vv.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace vv.Domain.Services
{
    public interface ICryptoMarketDataService
    {
        // Price retrieval operations
        Task<CryptoSpotPriceData> GetLatestPriceAsync(
            string exchange,
            string baseAsset,
            string quoteAsset,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<CryptoSpotPriceData>> GetHistoricalPricesAsync(
            string exchange,
            string baseAsset,
            string quoteAsset,
            DateOnly fromDate,
            DateOnly toDate,
            CancellationToken cancellationToken = default);

        // Market analysis operations
        Task<decimal> CalculateVolatilityAsync(
            string exchange,
            string baseAsset,
            string quoteAsset,
            int days,
            CancellationToken cancellationToken = default);

        Task<Dictionary<string, decimal>> GetMarketCapitalizationAsync(
            string[] assets = null,
            CancellationToken cancellationToken = default);

        // Cross-exchange operations
        Task<Dictionary<string, decimal>> GetArbitrageOpportunitiesAsync(
            string baseAsset,
            string quoteAsset,
            string[] exchanges,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<CryptoSpotPriceData>> GetCrossExchangePricesAsync(
            string baseAsset,
            string quoteAsset,
            string[] exchanges = null,
            CancellationToken cancellationToken = default);
    }
}