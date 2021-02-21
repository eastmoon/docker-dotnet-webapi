namespace WebService.Core.Common.Enums
{
    public enum HttpStatusCode
    {
        #region 1xx 表示服務內部發生異常，未明確定義的異常預設為 100。

        /// <summary>
        /// 服務內部發生異常。
        /// </summary>
        ServiceInternalError = 100,

        #endregion

        #region 2xx 表示平台傳送至本服務的資料內容異常

        /// <summary>
        /// 授權失敗
        /// </summary>
        AuthorizationFailed = 200,

        /// <summary>
        /// 找不到資源。
        /// </summary>
        ResourceNotFound = 201,

        /// <summary>
        /// 找不到平台。
        /// </summary>
        PlatformNotFound = 202

        #endregion

        #region 3xx 表示通知的資料內容異常
        #endregion

        #region 4xx 表示服務呼叫渠道時發生異常
        #endregion
    }
}
