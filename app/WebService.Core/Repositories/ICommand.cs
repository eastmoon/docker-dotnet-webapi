using System.Threading.Tasks;

namespace WebService.Core.Repositories
{
    public interface ICommand<TEntity>
        where TEntity : class
    {
        TEntity Create(TEntity entity);

        Task<TEntity> CreateAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
