﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebService.Core.Repositories
{
    public abstract class CRUDCommand<TEntity> : ICommand<TEntity>
        where TEntity : class
    {
        public IUnitOfWork UnitOfWork { get; set; }

        protected DbContext Context { get; }

        protected DbSet<TEntity> Set => Context.Set<TEntity>();

        protected CRUDCommand(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Context = unitOfWork.Context;
        }

        public TEntity Create(TEntity entity)
        {
            var entityEntry = Set.Add(entity);

            return (TEntity)entityEntry.CurrentValues.ToObject();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var entityEntry = await Set.AddAsync(entity);

            return (TEntity)entityEntry.CurrentValues.ToObject();
        }

        public void Update(TEntity entity)
        {
            Set.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            Set.Remove(entity);
        }
    }
}
