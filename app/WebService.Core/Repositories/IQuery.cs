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
        TEntity Find(uint sn);

        Task<TEntity> FindAsync(uint sn);

        TEntity Find(Guid uuid);

        Task<TEntity> FindAsync(Guid uuid);

        IQueryable<TEntity> FindAll();

        Result<TEntity> FindAll(int startIndex, int count);

        IQueryable<TEntity> FindAll(
            params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> FindAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includes);

        Task<Result<TEntity>> FindAllAsync(
            int startIndex,
            int count,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);

        Result<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate, int startIndex, int count);

        Task<IEnumerable<TEntity>> FindByConditionAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        Task<Result<TEntity>> FindByConditionAsync(
            int startIndex,
            int count,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
    }
}
