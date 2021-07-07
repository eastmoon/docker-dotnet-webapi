using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using WebService.Core.Mvc.Models;
using WebService.Core.Repositories;

namespace WebService.Core.Services
{
    public class CRUDService<TModel, TEntity> : UnitOfWorkService, ICRUDService<TModel>
        where TModel : Persistence
        where TEntity : class
    {
        protected CRUDRepository<TEntity> Repository { get; }
        public CRUDService(CRUDRepository<TEntity> repository, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            Repository = repository;
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

        public virtual TModel Find(uint id)
        {
            var entity = Repository.Query.Find(id);

            var model = MapToModel(entity);

            return model;
        }

        public virtual async Task<TModel> FindAsync(uint id)
        {
            var entity = await Repository.Query.FindAsync(id);

            var model = MapToModel(entity);

            return model;
        }

        public virtual IEnumerable<TModel> FindAll()
        {
            var entities = Repository.Query.FindAll();

            var models = MapToModel(entities);

            return models;
        }

        public virtual Result<TModel> FindAll(int startIndex, int count)
        {
            var entities = Repository.Query.FindAll(startIndex, count);

            var models = MapToModel(entities);

            return models;
        }

        #endregion

        #region Command

        public virtual TModel Create(TModel model)
        {
            model.CreateTime = DateTime.Now;

            var entity = MapToEntity(model);

            entity = Repository.Command.Create(entity);

            return MapToModel(entity);
        }

        public virtual async Task<TModel> CreateAsync(TModel model)
        {
            model.CreateTime = DateTime.Now;

            var entity = MapToEntity(model);

            entity = await Repository.Command.CreateAsync(entity);

            return MapToModel(entity);
        }

        public virtual void Update(TModel model)
        {
            var entity = MapToEntity(model);

            Repository.Command.Update(entity);
        }

        public virtual void Delete(TModel model)
        {
            var entity = MapToEntity(model);

            Repository.Command.Delete(entity);
        }

        #endregion
    }
}
