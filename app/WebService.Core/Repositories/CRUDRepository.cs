using System;
using System.Collections.Generic;
using System.Text;
using WebService.Entities.Context;

namespace WebService.Core.Repositories
{
    public class CRUDRepository<TEntity> : Repository<TEntity, Query<TEntity>, Command<TEntity>>
        where TEntity: class
    {
        public CRUDRepository(QueryDBContext queryContext, CommandDBContext commandContext) : 
            base(
                new Query<TEntity>(queryContext), 
                new Command<TEntity>(commandContext))
        {
            
        }
    }
}
