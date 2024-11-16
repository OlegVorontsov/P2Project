namespace P2Project.Domain.Shared
{
    public interface ISoftDeletable
    {
        public void Deleted();
        public void Restored();
    }
}
