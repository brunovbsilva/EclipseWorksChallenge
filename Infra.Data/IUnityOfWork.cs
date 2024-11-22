namespace Infra.Data
{
    public interface IUnitOfWork
    {
        Context Context { get; }
        Task SaveChangesAsync();
    }
}