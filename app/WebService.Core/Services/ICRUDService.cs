﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebService.Core.Models;

namespace WebService.Core.Services
{
    public interface ICRUDService<TModel>
        where TModel : class
    {
        TModel Find(uint sn);

        Task<TModel> FindAsync(uint sn);

        TModel Find(Guid uuid);

        Task<TModel> FindAsync(Guid uuid);

        IEnumerable<TModel> FindAll();

        Result<TModel> FindAll(int startIndex, int count);

        TModel Create(TModel model);

        Task<TModel> CreateAsync(TModel model);

        void Update(TModel model);

        void Delete(TModel model);
    }
}
