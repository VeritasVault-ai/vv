using System;
using System.Threading;
using System.Threading.Tasks;

namespace vv.Data.Repositories
{
    /// <summary>
    /// Defines a repository context that manages the database connection and transaction state
    /// </summary>
    public interface IRepositoryContext : IDisposable
    {
        /// <summary>
        /// Gets the database name
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// Begins a transaction that can be committed or rolled back
        /// </summary>
        Task<ITransactionScope> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Ensures the database and any required containers/collections exist
        /// </summary>
        Task EnsureDatabaseCreatedAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Resets the database connection
        /// </summary>
        Task ResetConnectionAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a transaction scope that can be committed or rolled back
    /// </summary>
    public interface ITransactionScope : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Commits the transaction
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}