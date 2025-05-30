using Microsoft.Extensions.Logging;
using vv.Domain.Models;
using vv.Domain.Repositories;
using vv.Domain.Repositories.Components;
using vv.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace vv.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of query operations for market data
    /// </summary>
    public class MarketDataQueries : IMarketDataQueries
    {
        private readonly ILogger<MarketDataQueries> _logger;
        private readonly IRepository<FxSpotPriceData> _repository;
        private readonly IVersioningCapability<FxSpotPriceData> _versioning;

        public MarketDataQueries(
            IRepository<FxSpotPriceData> repository,
            IVersioningCapability<FxSpotPriceData> versioning,
            ILogger<MarketDataQueries> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _versioning = versioning ?? throw new ArgumentNullException(nameof(versioning));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<FxSpotPriceData?> GetLatestMarketDataAsync(
            string dataType,
            string assetClass,
            string assetId,
            string region,
            DateOnly asOfDate,
            string documentType,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Retrieving latest market data: DataType={DataType}, AssetClass={AssetClass}, AssetId={AssetId}, Region={Region}, AsOf={AsOf}, DocType={DocType}",
                dataType, assetClass, assetId, region, asOfDate, documentType);

            // Use specification pattern
            var spec = new MarketDataSpecification()
                .WithDataType(dataType)
                .WithAssetClass(assetClass)
                .WithAssetId(assetId)
                .WithRegion(region)
                .WithAsOfDate(asOfDate)
                .WithDocumentType(documentType);

            // Use the versioning component
            var result = await _versioning.GetByLatestVersionAsync(spec, cancellationToken);
            return result.Result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FxSpotPriceData>> QueryByExpressionAsync(
            Expression<Func<FxSpotPriceData, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Executing expression query on market data");

            // Delegate to the repository component
            return await _repository.QueryAsync(predicate, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FxSpotPriceData>> QueryByRangeAsync(
            string dataType,
            string assetClass,
            string? assetId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Querying market data by range: DataType={DataType}, AssetClass={AssetClass}, AssetId={AssetId}, FromDate={FromDate}, ToDate={ToDate}",
                dataType, assetClass, assetId ?? "any", fromDate, toDate);

            // Convert DateTime to DateOnly if provided
            DateOnly? fromDateOnly = fromDate.HasValue ? DateOnly.FromDateTime(fromDate.Value) : null;
            DateOnly? toDateOnly = toDate.HasValue ? DateOnly.FromDateTime(toDate.Value) : null;

            // Use specification pattern
            var spec = new MarketDataSpecification()
                .WithDataType(dataType)
                .WithAssetClass(assetClass);

            if (!string.IsNullOrEmpty(assetId))
                spec.WithAssetId(assetId);

            if (fromDateOnly.HasValue)
                spec.WithFromDate(fromDateOnly.Value);

            if (toDateOnly.HasValue)
                spec.WithToDate(toDateOnly.Value);

            // Delegate to repository component
            return await _repository.QueryAsync(spec.ToExpression(), cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<(FxSpotPriceData? Result, string? ETag)> GetBySpecifiedVersionAsync(
            string dataType,
            string assetClass,
            string assetId,
            string region,
            DateOnly asOfDate,
            string documentType,
            int version,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Retrieving specific version of market data: DataType={DataType}, AssetClass={AssetClass}, AssetId={AssetId}, Region={Region}, AsOf={AsOf}, DocType={DocType}, Version={Version}",
                dataType, assetClass, assetId, region, asOfDate, documentType, version);

            // Use specification pattern
            var spec = new MarketDataSpecification()
                .WithDataType(dataType)
                .WithAssetClass(assetClass)
                .WithAssetId(assetId)
                .WithRegion(region)
                .WithAsOfDate(asOfDate)
                .WithDocumentType(documentType);

            // Delegate to versioning component
            return await _versioning.GetBySpecifiedVersionAsync(spec, version, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<(FxSpotPriceData? Result, string? ETag)> GetByLatestVersionAsync(
            string dataType,
            string assetClass,
            string assetId,
            string region,
            DateOnly asOfDate,
            string documentType,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Retrieving latest version of market data: DataType={DataType}, AssetClass={AssetClass}, AssetId={AssetId}, Region={Region}, AsOf={AsOf}, DocType={DocType}",
                dataType, assetClass, assetId, region, asOfDate, documentType);

            // Use specification pattern
            var spec = new MarketDataSpecification()
                .WithDataType(dataType)
                .WithAssetClass(assetClass)
                .WithAssetId(assetId)
                .WithRegion(region)
                .WithAsOfDate(asOfDate)
                .WithDocumentType(documentType);

            // Delegate to versioning component
            return await _versioning.GetByLatestVersionAsync(spec, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FxSpotPriceData>> QueryAsync(
            string dataType,
            string assetClass,
            string? assetId = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Querying market data: DataType={DataType}, AssetClass={AssetClass}, AssetId={AssetId}, FromDate={FromDate}, ToDate={ToDate}",
                dataType, assetClass, assetId ?? "any", fromDate, toDate);

            // Use specification pattern
            var spec = new MarketDataSpecification()
                .WithDataType(dataType)
                .WithAssetClass(assetClass);

            if (!string.IsNullOrEmpty(assetId))
                spec.WithAssetId(assetId);

            if (fromDate.HasValue)
                spec.WithFromDate(fromDate.Value);

            if (toDate.HasValue)
                spec.WithToDate(toDate.Value);

            // Delegate to repository component
            return await _repository.QueryAsync(spec.ToExpression(), cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<FxSpotPriceRate?> GetLatestExchangeRateAsync(
            string baseCurrency,
            string quoteCurrency,
            DateOnly asOfDate,
            CancellationToken cancellationToken = default)
        {
            // Domain-specific method implementation
            var spec = MarketDataSpecification.ForCurrencyPair(baseCurrency, quoteCurrency)
                .WithAsOfDate(asOfDate);

            var result = await _versioning.GetByLatestVersionAsync(spec, cancellationToken);
            return FxSpotPriceRate.FromEntity(result.Result);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FxSpotPriceData>> GetExchangeRateHistoryAsync(
            string baseCurrency,
            string quoteCurrency,
            DateOnly fromDate,
            DateOnly toDate,
            CancellationToken cancellationToken = default)
        {
            // Domain-specific method implementation
            var spec = MarketDataSpecification.ForCurrencyPair(baseCurrency, quoteCurrency)
                .WithFromDate(fromDate)
                .WithToDate(toDate);

            return await _repository.QueryAsync(spec.ToExpression(), cancellationToken: cancellationToken);
        }
    }
}