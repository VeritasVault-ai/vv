using System;
using System.Collections.Generic;
using MediatR;
using vv.Domain.Models;

namespace vv.Application.Queries
{
    public record QueryMarketDataQuery(
        string AssetClass,
        string? AssetId = null,
        DateOnly? FromDate = null,
        DateOnly? ToDate = null,
        string? Region = null) : IRequest<IEnumerable<FxSpotPriceData>>;
}