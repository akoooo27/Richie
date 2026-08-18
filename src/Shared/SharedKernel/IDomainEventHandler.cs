using Mediator;

namespace SharedKernel;

public interface IDomainEventHandler<in T> : INotificationHandler<T>
    where T : IDomainEvent
{
}
