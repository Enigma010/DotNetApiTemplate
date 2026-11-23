namespace Ddd.App.UnitOfWork
{
    public interface IUnitOfWork
    {
        public Task Begin();
        public Task Commit();
        public Task Rollback();
        public bool UseScopedTransactions { get; }
    }
}
