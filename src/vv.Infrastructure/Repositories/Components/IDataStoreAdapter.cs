using vv.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace vv.Domain.Repositories.Components
{
    /// <summary>
    /// Interface for data store specific operations
    /// </summary>
    public interface IDataStoreAdapter<T> where T : IMarketDataEntity
    {
        /// <summary>
        /// Gets an item with its ETag
        /// </summary>
        Task<(T? Entity, string? ETag)> GetItemWithETagAsync(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets just the ETag for an item
        /// </summary>
        Task<string?> GetETagAsync(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new item in the data store
        /// </summary>
        Task<T> CreateItemAsync(
            T entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces an existing item
        /// </summary>
        Task<T> ReplaceItemAsync(
            T entity,
            string? etag = null,
            CancellationToken cancellationToken = default);
    }
}