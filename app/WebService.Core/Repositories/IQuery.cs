using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebService.Core.Mvc.Models;

namespace WebService.Core.Repositories
{
    public interface IQuery<TEntity>
        where TEntity : class
    {
        TEntity Find(uint id);

        Task<TEntity> FindAsync(uint id);

        IQueryable<TEntity> FindAll();

        Result<TEntity> FindAll(int startIndex, int count);
    }
}
