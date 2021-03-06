﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebService.Core.Mvc.Models;

namespace WebService.Core.Services
{
    public interface ICRUDService<TModel> : IUnitOfWorkService
        where TModel : class
    {
        TModel Find(uint id);

        Task<TModel> FindAsync(uint id);

        IEnumerable<TModel> FindAll();

        Result<TModel> FindAll(int startIndex, int count);

        TModel Create(TModel model);

        Task<TModel> CreateAsync(TModel model);

        void Update(TModel model);

        void Delete(TModel model);
    }
}
