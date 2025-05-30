namespace vv.Domain.Models
{
    /// <summary>
    /// Interface for entities that support versioning
    /// </summary>
    public interface IVersionedEntity
    {
        /// <summary>
        /// Gets or sets the version number
        /// </summary>
        int Version { get; set; }

        /// <summary>
        /// Gets or sets the base version ID
        /// </summary>
        string? BaseVersionId { get; set; }

        /// <summary>
        /// Gets or sets whether this entity is the latest version
        /// </summary>
        bool IsLatestVersion { get; set; }
    }
}
