using System;

namespace vv.Domain.Events
{
    public class EntityCreatedEvent<T> : DomainEvent
    {
        public T Entity { get; }

        public EntityCreatedEvent(T entity)
        {
            Entity = entity;
            Source = "Repository";
        }
    }

    public class EntityUpdatedEvent<T> : DomainEvent
    {
        public T Entity { get; }

        public EntityUpdatedEvent(T entity)
        {
            Entity = entity;
            Source = "Repository";
        }
    }

    public class EntityDeletedEvent<T> : DomainEvent
    {
        public string EntityId { get; }

        public EntityDeletedEvent(string entityId)
        {
            EntityId = entityId;
            Source = "Repository";
        }
    }
}
