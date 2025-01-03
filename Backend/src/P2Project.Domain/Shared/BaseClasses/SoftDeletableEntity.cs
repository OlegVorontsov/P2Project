namespace P2Project.Domain.Shared.BaseClasses;

public class SoftDeletableEntity<TId> : Entity<TId> where TId : notnull
{
    protected SoftDeletableEntity(TId id) : base(id) { }
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletionDateTime { get; protected set; }

    public virtual void SoftDelete()
    {
        IsDeleted = true;
        DeletionDateTime = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletionDateTime = null;
    }
}