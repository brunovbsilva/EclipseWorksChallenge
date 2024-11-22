namespace Infra.Data
{
    public class UnitOfWork(Context context) : IUnitOfWork, IDisposable
    {
        public Context Context { get; set; } = context;
        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing) Context.Dispose();
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

