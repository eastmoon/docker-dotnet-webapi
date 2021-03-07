using System;
using System.Collections.Generic;
using System.Text;

namespace WebService.Core.Repositories
{
    public abstract class Repository<TEntity, TQuery, TCommand>: IRepository<TEntity, TQuery, TCommand>
        where TEntity : class
        where TQuery : IQuery<TEntity>
        where TCommand : ICommand<TEntity>

    {
        public TQuery Query { get; }
        public TCommand Command { get; }
        public Repository(TQuery query, TCommand command)
        {
            Query = query;
            Command = command;
        }
    }
}
