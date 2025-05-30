using System;
using MediatR;
using vv.Domain.Models;

namespace vv.Application.Queries
{
    public record GetLatestMarketDataQuery(
        string DataType,
        string AssetClass,
        string AssetId,
        string Region,
        DateOnly AsOfDate,
        string DocumentType) : IRequest<FxSpotPriceData>;
}