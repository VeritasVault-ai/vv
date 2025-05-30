namespace vv.Domain.Models
{
    /// <summary>
    /// Interface for entities that support versioning
    /// </summary>
    public interface IVersionedEntity
    {
        /// <summary>
        /// Gets or sets the version number of the entity
        /// </summary>
        int Version { get; set; }
    }
}