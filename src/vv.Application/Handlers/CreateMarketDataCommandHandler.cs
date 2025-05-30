using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using vv.Application.Commands;
using vv.Application.Services;

namespace vv.Application.Handlers
{
    public class CreateMarketDataCommandHandler : IRequestHandler<CreateMarketDataCommand, string>
    {
        private readonly IMarketDataService _marketDataService;
        private readonly ILogger<CreateMarketDataCommandHandler> _logger;

        public CreateMarketDataCommandHandler(
            IMarketDataService marketDataService,
            ILogger<CreateMarketDataCommandHandler> logger)
        {
            _marketDataService = marketDataService ?? throw new ArgumentNullException(nameof(marketDataService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(CreateMarketDataCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateMarketDataCommand for {AssetId}", request.Data.AssetId);
            return await _marketDataService.CreateMarketDataAsync(request.Data);
        }
    }
}