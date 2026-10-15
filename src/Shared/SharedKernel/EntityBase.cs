namespace SharedKernel;

public abstract class EntityBase : HasDomainEventsBase
{
    public int Id { get; set; }
}

public abstract class EntityBase<TId> : HasDomainEventsBase
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; set; } = default!;
}

public abstract class EntityBase<T, TId> : HasDomainEventsBase
    where T : EntityBase<T, TId>
{
    public TId Id { get; set; } = default!;
}