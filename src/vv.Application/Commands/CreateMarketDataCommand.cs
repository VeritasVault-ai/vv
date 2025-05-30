using MediatR;
using vv.Domain.Models;

namespace vv.Application.Commands
{
    public record CreateMarketDataCommand(FxSpotPriceData Data) : IRequest<string>;
}