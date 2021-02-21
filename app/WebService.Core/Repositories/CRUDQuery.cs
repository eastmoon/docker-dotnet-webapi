using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebService.Core.Models;

namespace WebService.Core.Repositories
{
    public abstract class CRUDQuery<TEntity> : IQuery<TEntity>
        where TEntity : class
    {
        protected DbContext Context { get; }

        protected IQueryable<TEntity> Set { get; set; }

        protected CRUDQuery(DbContext context)
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

        protected Expression<Func<TEntity, bool>> AndAlso(Expression<Func<TEntity, bool>> expr1,
            Expression<Func<TEntity, bool>> expr2)
        {
            // need to detect whether they use the same
            // parameter instance; if not, they need fixing
            var param = expr1.Parameters[0];

            return Expression.Lambda<Func<TEntity, bool>>(ReferenceEquals(param, expr2.Parameters[0])
                ? Expression.AndAlso(expr1.Body, expr2.Body)
                : Expression.AndAlso(expr1.Body, Expression.Invoke(expr2, param)), param); // otherwise, keep expr1 "as is" and invoke expr2
        }

        private IQueryable<TEntity> GetRelatedEntities(IQueryable<TEntity> set,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var include = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(x => x.Name == "Include" && x.GetParameters()
                    .Select(y => y.ParameterType.GetGenericTypeDefinition())
                    .SequenceEqual(new[]
                    {
                        typeof(IQueryable<>), typeof(Expression<>
                        )
                    })
                );

            var thenIncludeCollection = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Single(mi => !mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

            var lastThenIncludeInfo = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .Where(info => info.Name == "ThenInclude")
                .ToList()[0];

            foreach (var arg in includes)
            {
                // Retrieve all selection from input expression
                var lambda = (LambdaExpression)arg;
                var method = arg.Body as MethodCallExpression;
                var result = new List<Expression>();
                while (method != null)
                {
                    result.Add(Expression.Lambda(method.Arguments[0], lambda.Parameters[0]));
                    lambda = (LambdaExpression)method.Arguments[1];
                    method = lambda.Body as MethodCallExpression;
                }

                result.Add(lambda);

                if (result.Count > 0)
                {
                    for (var i = 0; i < result.Count; ++i)
                    {
                        var lambdaExp = (LambdaExpression)result[i];

                        if (i == 0)
                        {
                            set = include
                                .MakeGenericMethod(lambdaExp.Parameters[0].Type, lambdaExp.ReturnType)
                                .Invoke(null, new object[] { set, lambdaExp }) as IQueryable<TEntity>;
                        }
                        else if (i == result.Count - 1)
                        {
                            set = lastThenIncludeInfo
                                .MakeGenericMethod(((LambdaExpression)result[0]).Parameters[0].Type,
                                    lambdaExp.Parameters[0].Type, lambdaExp.ReturnType)
                                .Invoke(null, new object[] { set, lambdaExp }) as IQueryable<TEntity>;
                        }
                        else
                        {
                            set = thenIncludeCollection
                                .MakeGenericMethod(((LambdaExpression)result[0]).Parameters[0].Type,
                                    lambdaExp.Parameters[0].Type, lambdaExp.ReturnType)
                                .Invoke(null, new object[] { set, lambdaExp }) as IQueryable<TEntity>;
                        }
                    }
                }
            }

            return set;
        }

        #endregion

        #region Find

        public abstract TEntity Find(uint sn);

        public abstract Task<TEntity> FindAsync(uint sn);

        public abstract TEntity Find(Guid uuid);

        public abstract Task<TEntity> FindAsync(Guid uuid);

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

        public IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return FindAll(null, includes);
        }

        public IQueryable<TEntity> FindAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var relatedEntities = GetRelatedEntities(Set, includes);

            return orderBy != null
                ? orderBy(relatedEntities)
                : relatedEntities;
        }

        public async Task<Result<TEntity>> FindAllAsync(
            int startIndex,
            int count,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return await PagingAsync(FindAll(orderBy, includes), startIndex, count);
        }

        #endregion

        #region FindByCondition

        private IQueryable<TEntity> FindByCondition(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var set = Set.Where(predicate);
            var relatedEntities = GetRelatedEntities(set, includes);

            return orderBy != null
                ? orderBy(relatedEntities)
                : relatedEntities;
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Where(predicate);
        }

        public Result<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate, int startIndex,
            int count)
        {
            return Paging(FindByCondition(predicate), startIndex, count);
        }

        public async Task<IEnumerable<TEntity>> FindByConditionAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return await FindByCondition(predicate, orderBy, includes).ToListAsync();
        }

        public async Task<Result<TEntity>> FindByConditionAsync(
            int startIndex,
            int count,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return await PagingAsync(FindByCondition(predicate, orderBy, includes), startIndex, count);
        }

        #endregion
    }
}