using System.Threading.Tasks;

namespace vv.Domain.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(DomainEvent @event);
    }
}
