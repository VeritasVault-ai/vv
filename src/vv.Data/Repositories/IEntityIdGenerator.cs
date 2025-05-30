using vv.Domain.Models;

namespace vv.Data.Repositories
{
    /// <summary>
    /// Interface for entity ID generators
    /// </summary>
    public interface IEntityIdGenerator<T> where T : IMarketDataEntity
    {
        /// <summary>
        /// Generates a unique ID for the given entity
        /// </summary>
        /// <param name="entity">The entity for which to generate an ID</param>
        /// <returns>A unique ID string</returns>
        string GenerateId(T entity);
    }
}