using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebService.Core.Services;
using WebService.Core.Models;

namespace WebService.Core.Controllers
{
    /// <summary>
    /// WebService 專案 Controller 抽象基底類別。所有 API Controller 皆應繼承此類別。
    /// </summary>
    /// <remarks>
    /// 提供開發 WebService API 的公用函式庫。
    /// </remarks>
    public abstract class WSControllerBase : ControllerBase
    {
        protected ILogger Logger { get; }
        protected IUnitOfWorkService UnitOfWorkService { get; }

        /// <summary>
        /// 建構子。
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWorkService"></param>
        protected WSControllerBase(ILogger logger, IUnitOfWorkService unitOfWorkService)
        {
            Logger = logger;
            UnitOfWorkService = unitOfWorkService;
        }

        /// <summary>
        /// 非同步回傳成功狀態及資料內容，並完成原子操作。
        /// </summary>
        /// <typeparam name="TContent"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<Response<TContent>> Success<TContent>(TContent content)
        {
            await UnitOfWorkService.SaveAsync();

            return new Response<TContent>(content)
            {
                Success = true
            };
        }

        /// <summary>
        /// 非同步回傳成功狀態，並完成原子操作。
        /// </summary>
        /// <returns></returns>
        protected async Task<Response> Success()
        {
            await UnitOfWorkService.SaveAsync();

            return new Response
            {
                Success = true
            };
        }
    }
}
