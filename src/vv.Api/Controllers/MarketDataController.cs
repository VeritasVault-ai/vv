using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using vv.Application.Commands;
using vv.Application.Queries;
using vv.Domain.Models;

namespace vv.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MarketDataController> _logger;

        public MarketDataController(IMediator mediator, ILogger<MarketDataController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the latest market data for a specific asset
        /// </summary>
        /// <param name="assetClass">Asset class (e.g., "fx")</param>
        /// <param name="assetId">Asset identifier (e.g., "eurusd")</param>
        /// <param name="date">Reference date (optional, defaults to today)</param>
        /// <returns>The latest market data for the asset</returns>
        [HttpGet("{assetClass}/{assetId}/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FxSpotPriceData>> GetLatestMarketData(
            string assetClass,
            string assetId,
            [FromQuery] DateOnly? date = null)
        {
            _logger.LogInformation("Getting latest data for {AssetClass}/{AssetId}", assetClass, assetId);

            var query = new GetLatestMarketDataQuery(
                "price.spot",
                assetClass,
                assetId,
                "global",
                date ?? DateOnly.FromDateTime(DateTime.Today),
                "official");

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new market data entry
        /// </summary>
        /// <param name="command">The create command with market data</param>
        /// <returns>The ID of the created market data</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CreateMarketData(CreateMarketDataCommand command)
        {
            _logger.LogInformation("Creating market data for {AssetId}", command.Data.AssetId);

            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetLatestMarketData),
                new { assetClass = command.Data.AssetClass, assetId = command.Data.AssetId },
                result);
        }

        /// <summary>
        /// Queries market data based on parameters
        /// </summary>
        /// <param name="query">The query parameters</param>
        /// <returns>A collection of market data matching the query</returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FxSpotPriceData>>> QueryMarketData([FromQuery] QueryMarketDataQuery query)
        {
            _logger.LogInformation("Querying market data");

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}