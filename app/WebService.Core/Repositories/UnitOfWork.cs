using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebService.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        public DbContext Context { get; }

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Context.Dispose();
            }

            _disposed = true;
        }

        public int Save()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
