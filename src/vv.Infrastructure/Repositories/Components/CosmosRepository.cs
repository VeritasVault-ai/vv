public class CosmosRepository<T> : IRepository<T>, IDataStoreAdapter<T> where T : class, IMarketDataEntity
{
    protected readonly Container _container;
    protected readonly ILogger _logger;
    protected readonly Func<T, string> _partitionKeyResolver;
    protected readonly IEventPublisher? _eventPublisher;

    public CosmosRepository(
        Container container,
        ILogger logger,
        IEventPublisher? eventPublisher = null,
        Func<T, string>? partitionKeyResolver = null)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventPublisher = eventPublisher;
        _partitionKeyResolver = partitionKeyResolver ?? (e => e.AssetId);
    }
    
    // Implement all IRepository<T> methods (copied from existing CosmosRepository)
    
    // Implement IDataStoreAdapter<T> methods
    public async Task<(T? Entity, string? ETag)> GetItemWithETagAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
            return (null, null);
            
        try
        {
            var partitionKey = DeterminePartitionKey(id);
            var response = await _container.ReadItemAsync<T>(id, partitionKey, cancellationToken: cancellationToken);
            return (response.Resource, response.ETag);
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return (null, null);
        }
    }
    
    public async Task<string?> GetETagAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await GetItemWithETagAsync(id, cancellationToken);
        return result.ETag;
    }
    
    public async Task<T> CreateItemAsync(T entity, CancellationToken cancellationToken = default)
    {
        var response = await _container.CreateItemAsync(entity, GetPartitionKey(entity), cancellationToken: cancellationToken);
        return response.Resource;
    }
    
    public async Task<T> ReplaceItemAsync(T entity, string? etag = null, CancellationToken cancellationToken = default)
    {
        var options = new ItemRequestOptions();
        if (!string.IsNullOrEmpty(etag))
            options.IfMatchEtag = etag;
            
        var response = await _container.ReplaceItemAsync(entity, entity.Id, GetPartitionKey(entity), 
            options, cancellationToken);
            
        return response.Resource;
    }
    
    // Helper methods
    protected PartitionKey GetPartitionKey(T entity)
    {
        return new PartitionKey(_partitionKeyResolver(entity));
    }
    
    protected virtual PartitionKey DeterminePartitionKey(string id)
    {
        return new PartitionKey(id);
    }
}