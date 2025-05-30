using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using vv.Domain.Events;
using vv.Domain.Models;

namespace vv.Infrastructure.IntegrationEvents
{
    /// <summary>
    /// Handles domain events and publishes them as integration events via MassTransit
    /// </summary>
    /// <typeparam name="T">The type of entity this handler processes</typeparam>
    public class DomainEventToIntegrationEventHandler<T> :
        INotificationHandler<EntityCreatedEvent<T>>,
        INotificationHandler<EntityUpdatedEvent<T>>,
        INotificationHandler<EntityDeletedEvent<T>>
        where T : class, IMarketDataEntity
    {
        private readonly IBus _bus;

        public DomainEventToIntegrationEventHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(EntityCreatedEvent<T> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new EntityCreatedIntegrationEvent<T>(notification.Entity);
            await _bus.Publish(integrationEvent, cancellationToken);
        }

        public async Task Handle(EntityUpdatedEvent<T> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new EntityUpdatedIntegrationEvent<T>(notification.Entity);
            await _bus.Publish(integrationEvent, cancellationToken);
        }

        public async Task Handle(EntityDeletedEvent<T> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new EntityDeletedIntegrationEvent<T>(notification.Id);
            await _bus.Publish(integrationEvent, cancellationToken);
        }
    }

    public class EntityCreatedIntegrationEvent<T> where T : IMarketDataEntity
    {
        public T Entity { get; }

        public EntityCreatedIntegrationEvent(T entity)
        {
            Entity = entity;
        }
    }

    public class EntityUpdatedIntegrationEvent<T> where T : IMarketDataEntity
    {
        public T Entity { get; }

        public EntityUpdatedIntegrationEvent(T entity)
        {
            Entity = entity;
        }
    }

    public class EntityDeletedIntegrationEvent<T> where T : IMarketDataEntity
    {
        public string Id { get; }

        public EntityDeletedIntegrationEvent(string id)
        {
            Id = id;
        }
    }
}