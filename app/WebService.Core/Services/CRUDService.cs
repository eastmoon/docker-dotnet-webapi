using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using WebService.Core.Models;
using WebService.Core.Repositories;

namespace WebService.Core.Services
{
    public abstract class CRUDService<TModel, TEntity, TQuery, TCommand> : ICRUDService<TModel>
        where TModel : CRUDPersistenceModel
        where TEntity : class
        where TQuery : IQuery<TEntity>
        where TCommand : ICommand<TEntity>
    {
        protected TQuery Query { get; }

        protected  TCommand Command { get; }

        protected CRUDService(TQuery query, TCommand command)
        {
            Query = query;
            Command = command;
        }

        protected virtual TEntity MapToEntity(TModel model)
        {
            return model.Adapt<TEntity>();
        }

        protected virtual TModel MapToModel(TEntity entity)
        {
            return entity.Adapt<TModel>();
        }

        protected virtual IEnumerable<TModel> MapToModel(IEnumerable<TEntity> entities)
        {
            return entities.Adapt<IEnumerable<TModel>>();
        }

        protected virtual Result<TModel> MapToModel(Result<TEntity> entities)
        {
            return entities.Adapt<Result<TModel>>();
        }

        #region Query

        public virtual TModel Find(uint sn)
        {
            var entity = Query.Find(sn);

            var model = MapToModel(entity);

            return model;
        }

        public virtual async Task<TModel> FindAsync(uint sn)
        {
            var entity = await Query.FindAsync(sn);

            var model = MapToModel(entity);

            return model;
        }

        public virtual TModel Find(Guid uuid)
        {
            var entity = Query.Find(uuid);

            var model = MapToModel(entity);

            return model;
        }

        public virtual async Task<TModel> FindAsync(Guid uuid)
        {
            var entity = await Query.FindAsync(uuid);

            var model = MapToModel(entity);

            return model;
        }

        public virtual IEnumerable<TModel> FindAll()
        {
            var entities = Query.FindAll();

            var models = MapToModel(entities);

            return models;
        }

        public virtual Result<TModel> FindAll(int startIndex, int count)
        {
            var entities = Query.FindAll(startIndex, count);

            var models = MapToModel(entities);

            return models;
        }

        #endregion

        #region Command

        public virtual TModel Create(TModel model)
        {
            model.Uuid = Guid.NewGuid();
            model.CreateTime = DateTime.Now;

            var entity = MapToEntity(model);

            entity = Command.Create(entity);

            return MapToModel(entity);
        }

        public virtual async Task<TModel> CreateAsync(TModel model)
        {
            model.Uuid = Guid.NewGuid();
            model.CreateTime = DateTime.Now;

            var entity = MapToEntity(model);

            entity = await Command.CreateAsync(entity);

            return MapToModel(entity);
        }

        public virtual void Update(TModel model)
        {
            var entity = MapToEntity(model);

            Command.Update(entity);
        }

        public virtual void Delete(TModel model)
        {
            var entity = MapToEntity(model);

            Command.Delete(entity);
        }

        #endregion
    }
}
