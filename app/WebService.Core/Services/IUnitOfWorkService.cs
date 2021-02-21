using System;
using System.Threading.Tasks;

namespace WebService.Core.Services
{
    public interface IUnitOfWorkService : IDisposable
    {
        int Save();

        Task<int> SaveAsync();
    }
}
