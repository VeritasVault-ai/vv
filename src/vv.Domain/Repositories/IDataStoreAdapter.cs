public interface IDataStoreAdapter<T> where T : IMarketDataEntity
{
    // Abstracts database-specific operations
    Task<(T? Entity, string? ETag)> GetItemWithETagAsync(string id, CancellationToken cancellationToken = default);
    Task<string?> GetETagAsync(string id, CancellationToken cancellationToken = default);
    Task<T> CreateItemAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> ReplaceItemAsync(T entity, string? etag = null, CancellationToken cancellationToken = default);
    // Other database-specific operations
}