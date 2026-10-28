namespace SharedKernel;

public sealed class DomainEventBase : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; private set; } = DateTimeOffset.UtcNow;
}
