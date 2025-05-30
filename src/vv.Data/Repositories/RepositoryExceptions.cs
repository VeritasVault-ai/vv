using System;

namespace vv.Data.Repositories
{
    /// <summary>
    /// Exception thrown when an entity with the same ID already exists
    /// </summary>
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string entityName, string id)
            : base($"{entityName} with ID '{id}' already exists.")
        {
        }
    }

    /// <summary>
    /// Exception thrown when an entity with the specified ID cannot be found
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, string id)
            : base($"{entityName} with ID '{id}' was not found.")
        {
        }
    }

    /// <summary>
    /// Exception thrown when a repository operation fails
    /// </summary>
    public class RepositoryOperationException : Exception
    {
        public RepositoryOperationException(string message) : base(message)
        {
        }

        public RepositoryOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a concurrency conflict occurs
    /// </summary>
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message) : base(message)
        {
        }

        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}