using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using vv.Application.Queries;
using vv.Application.Services;
using vv.Domain.Models;

namespace vv.Application.Handlers
{
    public class GetLatestMarketDataQueryHandler : IRequestHandler<GetLatestMarketDataQuery, FxSpotPriceData>
    {
        private readonly IMarketDataService _marketDataService;
        private readonly ILogger<GetLatestMarketDataQueryHandler> _logger;

        public GetLatestMarketDataQueryHandler(
            IMarketDataService marketDataService,
            ILogger<GetLatestMarketDataQueryHandler> logger)
        {
            _marketDataService = marketDataService ?? throw new ArgumentNullException(nameof(marketDataService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<FxSpotPriceData> Handle(GetLatestMarketDataQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetLatestMarketDataQuery for {AssetId}", request.AssetId);
            return await _marketDataService.GetLatestMarketDataAsync(
                request.DataType,
                request.AssetClass,
                request.AssetId,
                request.Region,
                request.AsOfDate,
                request.DocumentType);
        }
    }
}