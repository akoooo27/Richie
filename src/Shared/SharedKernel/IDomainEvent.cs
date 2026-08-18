using Mediator;

namespace SharedKernel;

public interface IDomainEvent : INotification
{
    DateTimeOffset OccurredAt { get; }
}
