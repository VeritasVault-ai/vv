namespace vv.Domain.Repositories
{
    /// <summary>
    /// Factory for creating repository components
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a query-focused repository for market data
        /// </summary>
        IMarketDataQueries CreateMarketDataQueries();

        /// <summary>
        /// Creates a command-focused repository for market data
        /// </summary>
        IMarketDataCommands CreateMarketDataCommands();
    }
}