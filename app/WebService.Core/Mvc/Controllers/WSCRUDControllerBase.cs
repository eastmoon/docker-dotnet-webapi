using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using WebService.Core.Mvc.Models;
using WebService.Core.Services;
using WebService.Core.Common.Exceptions;

namespace WebService.Core.Mvc.Controllers
{
    /// <summary>
    /// WebService 專案 Controller 泛型抽象基底類別，其包括標準 CRUD 操作的。
    /// </summary>
    /// <typeparam name="TRequest">新增和修改 API 的輸入物件。</typeparam>
    /// <typeparam name="TResponse">查詢 API 的輸出物件。</typeparam>
    /// <typeparam name="TModel">支援持久化的商業物件。</typeparam>
    public abstract class WSCRUDControllerBase<TRequest, TResponse, TModel> : WSControllerBase
        where TModel : Persistence
    {
        protected readonly ICRUDService<TModel> Service;

        protected WSCRUDControllerBase(
            ILogger logger,
            ICRUDService<TModel> service)
            : base(logger, service)
        {
            Service = service;
        }

        /// <summary>
        /// 非同步查詢所有資料。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Response<IEnumerable<TResponse>>> GetAsync()
        {
            var models = Service.FindAll();
            var viewModels = models.Adapt<IEnumerable<TResponse>>();

            return await Success(viewModels);
        }

        /// <summary>
        /// 依 ID 非同步查詢資料。
        /// </summary>
        /// <param name="id">要查詢的資料的 ID。</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<Response<TResponse>> GetAsync(uint id)
        {
            var model = await Service.FindAsync(id);
            var viewModel = model.Adapt<TResponse>();

            return await Success(viewModel);
        }

        /// <summary>
        /// 新增一筆資料。
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Response<uint>> PostAsync([FromBody] TRequest viewModel)
        {
            var model = viewModel.Adapt<TModel>();

            model = await Service.CreateAsync(model);

            return await Success(model.Id);
        }

        /// <summary>
        /// 修改指定的資料。
        /// </summary>
        /// <param name="id">要修改的資料的 ID。</param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut]
        public virtual async Task<Response> PutAsync(uint id, TRequest viewModel)
        {
            var model = await Service.FindAsync(id);

            if (model == null)
            {
                throw new ResourceNotFoundNsException(id);
            }

            viewModel.Adapt(model);

            Service.Update(model);

            return await Success();
        }

        /// <summary>
        /// 修改指定的資料。
        /// </summary>
        /// <param name="id">要刪除的資料的 ID。</param>
        /// <returns></returns>
        [HttpDelete]
        public virtual async Task<Response> Delete(uint id)
        {
            var model = await Service.FindAsync(id);

            if (model == null)
            {
                throw new ResourceNotFoundNsException(id);
            }

            Service.Delete(model);

            return await Success();
        }
    }
}
