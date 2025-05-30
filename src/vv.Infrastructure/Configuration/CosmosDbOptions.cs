using vv.Data;

namespace vv.Infrastructure.Configuration
{
    /// <summary>
    /// Configuration options for Cosmos DB
    /// </summary>
    public class CosmosDbOptions : DataStorageOptions
    {
        /// <summary>
        /// Gets or sets the container name for market data
        /// </summary>
        public string MarketDataContainerName { get; set; } = "MarketData";

        /// <summary>
        /// Gets or sets the container name for reference data
        /// </summary>
        public string ReferenceDataContainerName { get; set; } = "ReferenceData";

        /// <summary>
        /// Gets or sets whether to initialize the database on startup
        /// </summary>
        public bool CreateDatabaseIfNotExists { get; set; } = false;

        /// <summary>
        /// Gets or sets the throughput to provision when creating the database
        /// </summary>
        public int? InitialThroughput { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of concurrent operations
        /// </summary>
        public int MaxConcurrentOperations { get; set; } = 10;

        /// <summary>
        /// Gets or sets whether to enable bulk execution
        /// </summary>
        public bool EnableBulkExecution { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum number of retry attempts
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// Gets or sets the maximum wait time between retries in seconds
        /// </summary>
        public int MaxRetryWaitTimeInSeconds { get; set; } = 30;
    }
}