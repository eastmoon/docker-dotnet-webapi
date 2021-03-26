# .NET Core 應用程式生命週期

對於任何應用程式框架而言，依據其依循的架構概念、設計用途，可以區分為兩種類型：

+ 被動運作的函式庫集
+ 主動運作的樣板框架

前者顧名思義就是提供一個針對特定演算法的函式庫集，例如 OpenCV，後者是提供一個框架進入點，並依據編寫的類別、行為，來完整運作後的成果；而 .NET Core 應用程式便是一種主動運作的樣板框架，而此類樣板框架的運用原則，便是提供開發人員一系列的類別繼承 ( Inheritance )、行為覆載 ( Override )，框架則基於此樣板的演算原則來觸發類別與行為，而這個演算原則便稱呼為應用程式生命週期。

## 應用程式生命週期

編譯語言具體可分為兩種生命週期：

+ 編譯生命週期

程式在編譯前會依據設定對引入專案與設定專案有關的處理週期，主要用於 CI/CD 與相關整合設計

+ 執行生命週期

軟體編譯後並執行，在啟動一個網際網路服務並收到需求後的處理週期；在細節上可再區分為：

- 需求處理週期
- 相依注入週期

### 需求處理週期

.NET Core 應用程式的依據版本的生命週期在其細節會有差異，詳細參考文獻連結，在此引用整理後的流程進行說明。

![ASP.NET MVC Request lifecycle](./img/dotnet-mvc-request-lifecycle.png)

如上圖所示，ASP.NET MVC Request 生命週期：

+ Middleware

中間件 ( Middleware ) 在整個應用程式流程中，用於處理用戶定義的需求資訊前處理工作，設定於此的元件將會對每個進來的需求做統一的處理。

+ Routing

路由 ( Routing ) 是一個由 MVC 框架定義的中間件，此元件用於將需求分配到對應的控制器 ( Controller ) 與行為 ( Action )，而分配標準則依據約定路由 ( Convention routes ) 或屬性路由 ( Attribute routes )

+ Controller Initialization

初始化控制器，在此階段 MVC 框架會基於非同步任務 ( Task ) 來初始化控制器，並讓控制器處裡需求，但需注意，控制器會執行最接近需求的路由模板所對應的行為。

+ Action Method Execution

在完成控制器初始化並開啟任務後，MVC 框架便會執行行為並取得執行結果。

+ Resutl Execution

MVC 框架取回執行結果後，會依據回應物件分為資料結果、呈現結果，前者為 HTTP Response，後者會經過呈現繪圖 ( View Rendering ) 轉為 HTML Response。

詳細生命週期處裡細節可參考下圖所示內容，而對開發人員來說，實務設計會著重在：

+ Middleware，設計全域行為
+ Action，區域過濾器、行為邏輯設計
+ Result，回應格式、頁面設計

<center>
    <img width="50%" src="./img/dotnet-mvc-request-pipeline.png" alt="ASP.NET MVC Request lifecycle" />
</center>

> from [Detailed ASP.NET MVC Pipeline](https://www.dotnettricks.com/learn/mvc/detailed-aspnet-mvc-pipeline)


### 相依注入週期

### 文獻

+ [Lifecycle of an ASP.NET MVC 5 Application](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/lifecycle-of-an-aspnet-mvc-5-application)
    - [ASP.NET MVC 5 Application lifecycle PDF](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/lifecycle-of-an-aspnet-mvc-5-application/_static/lifecycle-of-an-aspnet-mvc-5-application1.pdf)
    - [ASP.NET Application Life Cycle Overview for IIS 7.0](https://docs.microsoft.com/en-us/previous-versions/bb470252(v=vs.140))
        + [ASP.NET Core 3 系列 - 程式生命週期 (Application Lifetime)](https://blog.johnwu.cc/article/asp-net-core-3-application-lifetime.html)
    - [ASP.NET Core MVC Request Life Cycle](https://www.c-sharpcorner.com/article/asp-net-core-mvc-request-life-cycle/)
        + [ASP.NET Core MVC Request Life Cycle](https://www.c-sharpcorner.com/article/asp-net-core-mvc-request-life-cycle/)
        + [淺談 ASP.NET MVC 的生命週期](https://nwpie.blogspot.com/2017/05/5-aspnet-mvc.html)
        + [ASP .NET Core MVC 生命週期](https://ithelp.ithome.com.tw/articles/10242725)
        + [ASP.NET Core in Action - What is middleware?](https://andrewlock.net/asp-net-core-in-action-what-is-middleware/)
+ [ASP.NET Core DI 生命週期 LifeTime](https://marcus116.blogspot.com/2019/04/netcore-aspnet-core-di-lifetime.html)

## Middleware、Filters、Models


### 文獻

+ [Middleware vs Filters ASP. NET Core](https://www.edgesidesolutions.com/middleware-vs-filters-asp-net-core/)
    - [Filter 和 Middleware](https://www.dotblogs.com.tw/Null/2020/03/19/120500)
    - Middleware
        + [ASP.NET Core 中介軟體](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1)
        + [撰寫自訂的 ASP.NET Core 中介軟體](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/write?view=aspnetcore-5.0)
        + [ASP.NET Core 基礎 - Middleware](https://blog.darkthread.net/blog/aspnetcore-middleware-lab/)
        + [.Net Core Project 從零開始 — Middleware的概念與應用](https://medium.com/@WilliamWhetstone/net-core-project-%E5%BE%9E%E9%9B%B6%E9%96%8B%E5%A7%8B-middleware%E7%9A%84%E6%A6%82%E5%BF%B5%E8%88%87%E6%87%89%E7%94%A8-cb426045050e)
    - Filters
        + [ASP.NET Core 中的篩選條件](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.1)
        + [ASP.NET Core MVC 過濾器介紹](https://www.twblogs.net/a/5c76851bbd9eee339918009c)
    - Models
        + [在 ASP.NET Core 中使用應用程式模型](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/application-model?view=aspnetcore-3.1)

## 驗證與授權

### 文獻

+ [ASP.NET Core 驗證的總覽](https://docs.microsoft.com/zh-tw/aspnet/core/security/authentication/?view=aspnetcore-3.1)
+ [ASP.NET Core 的授權簡介](https://docs.microsoft.com/zh-tw/aspnet/core/security/authorization/introduction?view=aspnetcore-3.1)
    + [ASP.NET Core 中以角色為基礎的授權](https://docs.microsoft.com/zh-tw/aspnet/core/security/authorization/roles?view=aspnetcore-3.1)
    + [ASP.NET Core 中以宣告為基礎的授權](https://docs.microsoft.com/zh-tw/aspnet/core/security/authorization/claims?view=aspnetcore-3.1)
    + [ASP.NET Core 中以原則為基礎的授權](https://docs.microsoft.com/zh-tw/aspnet/core/security/authorization/policies?view=aspnetcore-3.1)
