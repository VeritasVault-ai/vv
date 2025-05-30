using vv.Domain.Models;
using vv.Domain.Repositories;

namespace vv.Data.Repositories
{
    /// <summary>
    /// Factory interface for creating repositories
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a repository for the specified entity type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <typeparam name="TRepository">The repository type</typeparam>
        /// <returns>A repository instance</returns>
        TRepository CreateRepository<T, TRepository>()
            where T : class, IMarketDataEntity
            where TRepository : IRepository<T>;

        /// <summary>
        /// Creates a market data repository
        /// </summary>
        /// <returns>A market data repository</returns>
        IMarketDataRepository CreateMarketDataRepository();
    }
}