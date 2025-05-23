namespace P2Project.SharedKernel.BaseClasses;

public abstract class DomainEntity<TId> : Entity<TId>
    where TId : IComparable<TId>
{
    protected DomainEntity(TId id) : base(id) { }
    
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}