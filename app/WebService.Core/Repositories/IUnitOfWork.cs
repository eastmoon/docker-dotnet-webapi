using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebService.Entities.Context;

namespace WebService.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        CommandDBContext Context { get; }

        int Save();

        Task<int> SaveAsync();
    }
}
