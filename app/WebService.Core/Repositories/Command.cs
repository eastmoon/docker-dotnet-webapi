using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebService.Entities.Context;

namespace WebService.Core.Repositories
{
    public class Command<TEntity> : ICommand<TEntity>
        where TEntity : class
    {
        protected CommandDBContext Context { get; }

        protected DbSet<TEntity> Set => Context.Set<TEntity>();

        public Command(CommandDBContext context)
        {
            Context = context;
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
