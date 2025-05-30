using vv.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace vv.Domain.Repositories
{
    /// <summary>
    /// Interface for write operations on market data
    /// </summary>
    public interface IMarketDataCommands
    {
        /// <summary>
        /// Saves a market data entity with versioning
        /// </summary>
        Task<string> SaveAsync(
            FxSpotPriceData marketData,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a market data entity
        /// </summary>
        Task<bool> DeleteAsync(
            string id,
            bool soft = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Permanently removes soft-deleted entities
        /// </summary>
        Task<int> PurgeSoftDeletedAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves an exchange rate
        /// </summary>
        Task<string> SaveExchangeRateAsync(
            string baseCurrency,
            string quoteCurrency,
            decimal rate,
            DateOnly asOfDate,
            string region = "global",
            string documentType = "official",
            CancellationToken cancellationToken = default);
    }
}