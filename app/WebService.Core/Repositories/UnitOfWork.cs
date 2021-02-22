using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebService.Entities.Context;

namespace WebService.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        public CommandDBContext Context { get; }

        public UnitOfWork(CommandDBContext context)
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
