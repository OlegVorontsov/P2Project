namespace P2Project.SharedKernel.BaseClasses;

public class SoftDeletableEntity<TId> : Entity<TId> where TId : notnull
{
    protected SoftDeletableEntity(TId id) : base(id) { }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletionDateTime { get; private set; }

    public virtual void SoftDelete()
    {
        if(IsDeleted) return;
        
        IsDeleted = true;
        DeletionDateTime = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        if(!IsDeleted) return;

        IsDeleted = false;
        DeletionDateTime = null;
    }
}