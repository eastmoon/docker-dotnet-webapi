﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using WebService.Controllers;
using WebService.Core.Models;
using WebService.Core.Services;
using WebService.Core.Common.Exceptions;

namespace NotificationService.Controllers
{
    /// <summary>
    /// WebService 專案 Controller 泛型抽象基底類別，其包括標準 CRUD 操作的。
    /// </summary>
    /// <typeparam name="TRequest">新增和修改 API 的輸入物件。</typeparam>
    /// <typeparam name="TResponse">查詢 API 的輸出物件。</typeparam>
    /// <typeparam name="TModel">支援持久化的商業物件。</typeparam>
    /// <typeparam name="TService">提供標準的 CRUD 操作的服務層物件。</typeparam>
    public abstract class WSCRUDControllerBase<TRequest, TResponse, TModel, TService> : WSControllerBase
        where TModel : CRUDPersistenceModel
        where TService : ICRUDService<TModel>
    {
        protected readonly TService Service;

        protected WSCRUDControllerBase(
            ILogger logger,
            IUnitOfWorkService unitOfWorkService,
            TService service)
            : base(logger, unitOfWorkService)
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
        /// 非同步查詢指定的分頁。
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("paging/{startIndex}/{count}")]
        public virtual async Task<PagingResponse<TResponse>> GetAsync(int startIndex, int count)
        {
            var models = Service.FindAll(startIndex, count);
            var viewModels = models.Adapt<Result<TResponse>>();

            return await Success(viewModels);
        }

        /// <summary>
        /// 依 UUID 非同步查詢資料。
        /// </summary>
        /// <param name="uuid">要查詢的資料的 UUID。</param>
        /// <returns></returns>
        [HttpGet("{uuid}")]
        public virtual async Task<Response<TResponse>> GetAsync(Guid uuid)
        {
            var model = await Service.FindAsync(uuid);
            var viewModel = model.Adapt<TResponse>();

            return await Success(viewModel);
        }

        /// <summary>
        /// 新增一筆資料。
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<Response<Guid>> PostAsync([FromBody] TRequest viewModel)
        {
            var model = viewModel.Adapt<TModel>();

            model = await Service.CreateAsync(model);

            return await Success(model.Uuid);
        }

        /// <summary>
        /// 修改指定的資料。
        /// </summary>
        /// <param name="uuid">要修改的資料的 UUID。</param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut]
        public virtual async Task<Response> PutAsync(Guid uuid, TRequest viewModel)
        {
            var model = await Service.FindAsync(uuid);

            if (model == null)
            {
                throw new ResourceNotFoundNsException(uuid);
            }

            viewModel.Adapt(model);

            Service.Update(model);

            return await Success();
        }

        /// <summary>
        /// 修改指定的資料。
        /// </summary>
        /// <param name="uuid">要刪除的資料的 UUID。</param>
        /// <returns></returns>
        [HttpDelete]
        public virtual async Task<Response> Delete(Guid uuid)
        {
            var model = await Service.FindAsync(uuid);

            if (model == null)
            {
                throw new ResourceNotFoundNsException(uuid);
            }

            Service.Delete(model);

            return await Success();
        }
    }
}
