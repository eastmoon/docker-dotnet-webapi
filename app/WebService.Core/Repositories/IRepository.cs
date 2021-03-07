using System.Threading.Tasks;

namespace WebService.Core.Repositories
{
    public interface IRepository<TEntity, TQuery, TCommand>
        where TEntity : class
        where TQuery : IQuery<TEntity>
        where TCommand : ICommand<TEntity>
    {

        TQuery Query { get; }

        TCommand Command { get; }
    }
}
