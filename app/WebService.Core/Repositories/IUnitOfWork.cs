using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebService.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }

        int Save();

        Task<int> SaveAsync();
    }
}
