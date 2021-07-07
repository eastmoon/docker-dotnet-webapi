using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebService.Core.Mvc.Models;
using WebService.Entities.Context;

namespace WebService.Core.Repositories
{
    public class Query<TEntity> : IQuery<TEntity>
        where TEntity : class
    {
        protected QueryDBContext Context { get; }

        protected IQueryable<TEntity> Set { get; set; }

        public Query(QueryDBContext context)
        {
            Context = context;
            Set = context.Set<TEntity>().AsNoTracking();
        }

        #region Methods

        private static Result<TEntity> Paging(IQueryable<TEntity> query, int startIndex, int count)
        {
            var total = query.LongCount();

            var page = query
                .Skip(startIndex)
                .Take(count);

            var pageResult = new Result<TEntity>
            {
                Page = page,
                Total = total
            };

            return pageResult;
        }

        private static async Task<Result<TEntity>> PagingAsync(IQueryable<TEntity> query, int startIndex,
            int count)
        {
            var total = await query.LongCountAsync();

            var page = await query
                .Skip(startIndex)
                .Take(count)
                .ToListAsync();

            var pageResult = new Result<TEntity>
            {
                Page = page,
                Total = total
            };

            return pageResult;
        }
        #endregion

        #region Find

        public TEntity Find(uint id)
        {
            // Use reflection to retrieve property and property value
            // If property exist the run find action to retrieve data.
            if ( typeof(TEntity).GetProperty("Id") != null )
            {
                return Set.SingleOrDefault(o => (uint)o.GetType().GetProperty("Id").GetValue(o) == id);
            }
            return null;
        }

        public async Task<TEntity> FindAsync(uint id)
        {
            // Use reflection to retrieve property and property value
            // If property exist the run find action to retrieve data.
            if (typeof(TEntity).GetProperty("Id") != null)
            {
                return await Set.SingleOrDefaultAsync(o => (uint)o.GetType().GetProperty("Id").GetValue(o) == id);
            }
            return null;
        }
        #endregion

        #region FindAll

        public IQueryable<TEntity> FindAll()
        {
            return Set;
        }

        public Result<TEntity> FindAll(int startIndex, int count)
        {
            return Paging(FindAll(), startIndex, count);
        }
        #endregion
    }
}