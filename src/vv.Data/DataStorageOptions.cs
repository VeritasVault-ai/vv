namespace vv.Data
{
    /// <summary>
    /// Common options for data storage - database agnostic
    /// </summary>
    public class DataStorageOptions
    {
        /// <summary>
        /// Gets or sets the connection string to the data store
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database name
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the default page size for paged queries
        /// </summary>
        public int DefaultPageSize { get; set; } = 50;

        /// <summary>
        /// Gets or sets the maximum page size for paged queries
        /// </summary>
        public int MaxPageSize { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the default query timeout in seconds
        /// </summary>
        public int DefaultQueryTimeout { get; set; } = 30;
    }
}