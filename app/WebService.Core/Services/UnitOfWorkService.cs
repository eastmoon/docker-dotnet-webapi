using System.Threading.Tasks;
using WebService.Core.Repositories;

namespace WebService.Core.Services
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private bool _disposed;

        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _unitOfWork.Dispose();
            }

            _disposed = true;
        }

        public int Save()
        {
            return _unitOfWork.Save();
        }

        public async Task<int> SaveAsync()
        {
            return await _unitOfWork.SaveAsync();
        }
    }
}
