namespace SharedKernel;

public abstract class DomainEventBase : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; protected set; } = DateTimeOffset.UtcNow;
}