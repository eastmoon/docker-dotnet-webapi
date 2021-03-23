# .NET Core 架構

ASP.NET Web 應用程式架構，本文參考依據 .NET 3.1 與 .NET 5.0 相關文獻整理

+ [架構原則](https://docs.microsoft.com/zh-tw/dotnet/architecture/modern-web-apps-azure/architectural-principles)
+ [一般 Web 應用程式架構](https://docs.microsoft.com/zh-tw/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
+ [Advanced Architecture for ASP.NET Core Web API](https://www.infoq.com/articles/advanced-architecture-aspnet-core/)

## 架構原則

**「如果營造商要像程式設計人員撰寫程式那樣地蓋房子，那麼第一隻經過的啄木鳥將會摧毀文明。」- Gerald Weinberg**

.NET Core 其架構原則依循以下規範：

+ 擴展性
    - 關注點分離
    - 封裝
    - 不重複原則 (DRY)
    - 單一責任 ( SOLID )
+ 關係性
    - 相依性反轉
    - 明確相依性
+ 維護性
    - 續性無知 (PI)
    - 繫結內容

#### 擴展性

在軟體開發過程中，專案隨則需求增加，會逐漸地擴大專案的規模，進而導致專案中出現大量重複、相依、定義不明、無用的程式碼；因此，基於這點的假設，便是擴展性中提到的四個主要架構原則。

而其原則，簡單而論便是：

+ 依據職責區分類別，例如 MVC 中的 Model、View、Controller 會分開配置
+ 依據模組封裝類別，例如 使用者、天氣是兩個明確區分的功能，應將其分離後保留介面以供外部溝通
+ 程式應該精簡，並將重複行為、操作應抽象化，避免冗餘或重複的程式碼
+ 物件設計應符合 SOLID 原則，保持物件在低耦合、高內聚的設計原則

#### 關係性

在基於擴展性的 SOLID 中，其中 D 是指依賴反轉原則 ( Dependency inversion principle，DIP )，此原則最大原則是解耦，而其概念如下圖所示

![反轉相依示意圖](./img/image4-2.png)

簡單而論，依賴反轉原則是避免物件關聯在編譯期、執行期相同，其考量是假設執行期會依據不同環境關聯不同實體物件，因此若考量擴展性需要，就必須降低關聯的直接性；從而繁生出依賴反轉原則，而這原則在不同的編譯設計更是大量運用，諸如 node.js webpack、android grade、C++ cmake 都是以此原則在設計其中邏輯性，進而達到針對不同環境的編譯結果。

因此，若**依賴反轉**設計原則是一種將『關系性注入』的編譯期設計方式，讓類別間的關係是不明確且抽象的，那麼**明確相依性**就是指編譯轉換至執行時，必須明確的定義相依性原則，使得執行期的實例物件可依據抽象關係建構關係。

但是，弱型別語言中可以透過反射原則來動態掛入生成物件，可強行別的語言中，在編譯時會檢查類別關係，若改用抽象介面來迴避檢查，則必然會在某個地方完成實例此類別為物件的行為，而這樣設計的部分便會可能如下文所述的產生循環複雜度 ( Cyclomatic Complexity )，亦即編寫大量的基於條件的 ```new class()``` 行為。

+ [Advanced Architecture for ASP.NET Core Web API](https://www.infoq.com/articles/advanced-architecture-aspnet-core/)
+ [CA1502: Avoid excessive complexity](https://docs.microsoft.com/zh-tw/previous-versions/visualstudio/visual-studio-2015/code-quality/ca1502-avoid-excessive-complexity?view=vs-2015&redirectedfrom=MSDN)
+ [The Ports and Adapters Pattern, Kenneth Lange](https://www.kennethlange.com/ports-and-adapters/)

而其文中提到的 Ports and Adapters Pattern 便是透過對服務註冊介面與類別關係性，並在有注入需要時調用已經生成的實例物件。

```
public static class UsersServiceStartup
{
    public static IServiceCollection AddWSUsersService(this IServiceCollection services)
    {
        services.AddScoped<CRUDRepository<Users>, UsersRepository>();
        services.AddScoped<CRUDService<UsersModel, Users>, UsersService>();
        return services;
    }
}
public class UsersController : WSCRUDControllerBase<UsersRequest, UsersResponse, UsersModel>
{
    public UsersController(ILogger<UsersController> logger,CRUDService<UsersModel, Users> service) : base(logger, service) {...}
}
public class UsersService : CRUDService<UsersModel, Users>
{
    public UsersService(CRUDRepository<Users> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork) { ... }
}
```
> 上述範例中，系統先是宣告了 ```AddWSUsersService()``` 來建立介面與類別關係，當物件生成時，便會依據在建構式中宣告的介面來調用由系統生成的物件。

### 維護性

+ [Patterns in Practice - Persistence Patterns](https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/april/design-patterns-for-data-persistence)

在 Domain-Driven Design 中，將服務 ( Service ) 與資料 ( Respository ) 區分，前者代表著服務本身的邏輯與運算，後者代表對應的資料庫存取方式，而若專案專注於領域模型設計，則應將其重心放在服務，而非資料如何操作；因此，參考文獻 [設計 DDD 導向微服務](https://docs.microsoft.com/zh-tw/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice) 所述：

**『遵循 Persistence Ignorance (永續性無知) 與 Infrastructure Ignorance (基礎結構無知) 準則，此層必須完全忽略資料永續性詳細資料。 這些永續性工作應由基礎結構層執行。』**

基於前述概念實踐的便是 [Entity Framework Core](../app/WebService.Entities/readme.md)，在這結構中，對資料庫的操作可依賴工具動態生成，並可透過 CI/CD 架構來維繫可用的資料模組，減少對繁瑣重複行為的人工過程，以此讓開發人員專注於領域模型與商業邏輯的設計。

## ASP.NET 應用程式架構

### ASP.NET MVC Web Application

+ [一般 Web 應用程式架構](https://docs.microsoft.com/zh-tw/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)

Web Application 是一個經過時代演進後的名詞，早在 Flash 提出的 RIA ( Rich Interface Application ) 時，前端 ( Frontend )、後端 ( Backend ) 就已逐漸分離，然而一個完整的應用程式就必然同時存在兩者，僅是因為技術的逐漸演進，一個單機運作的應用程式，也早已規模化又細碎化後，在諸多不同領域各自展現其特長；亦如同最早的 ASP 與 IIS，發展至今的 ASP.NET Core。

因此，文中所述的**整合型應用程式**其概念便是一個將前端、後端整合的可獨立執行的網路應用程式 ( Web Application )，而 ASP.NET 專案依據 MVC 架構設計，其單一專案目錄可分為 Models、Views、Controllers 三部分，而依據其執行關係則如同下圖的三層 ( Layered ) 架構來描述：

![Application Layers](./img/image5-2.png)

+ User Interface，應用程式介面層
+ Business Logic，商業邏輯層
+ Data Access，資料存取層

其層級間的互動關係明確，並且可以確保各自職責分明；然而這種傳統分層方法的一項缺點是編譯時間相依性會從頂端一直到底部，這導致三層結構最終封裝於單一執行程式，導致測試、局部替換的複雜度提升，面對專案規模擴大，其通訊與系統負擔便會相對提高。

### Clean Architecture

**『Clean Architecture 會將商務邏輯和應用程式模型放在應用程式的中央位置。不讓商務邏輯相依於資料存取或其他基礎結構的關注點，而是反轉此相依性：基礎結構和實作詳細資料相依於應用程式核心。這項功能的達成方式是在應用程式核心中定義抽象概念或介面，然後由基礎結構層中定義的類型來執行。』**

ASP.NET Core 的架構演變就如同大多數網路應用程式相似，在單一專案的應用上可維持在傳統分層上，碰上服務、流量增加、介面頻繁變動、分散運算效率、資料存取不同步等問題後，變逐步將前端、後端分離，並測試為前提讓其獨立讓其運，直到執行階段再讓其依存運作。

![ASP.NET Core Clean Architecture](./img/image5-9.png)

在如上圖架構中，其結構分為三個部分：

+ ASP.NET Core Web App
ASP.NET 專案核心設定與定義

+ Application Core Project
應用程式核心，基於領域驅動設計 (DDD) 準則設計的商業邏輯核心

+ Infrastructure Project
應用程式基礎設定，資料存取、通訊服務、引用第三方工具設定

整合上述三部分，從執行面來看其網路應用程式單元便如下圖所示呈現。

![ASP.NET Core Architecture](./img/image5-12.png)

### .NET Core & MVC

MVC 在應用程式架構中，其主旨在明確區分資料、呈現、控制三者，並講述其關係，常見的變形設計如 DV ( Document-View )、MVP ( Models-Views-Presenter )、MVVM ( Models-Views-ViewModels )；而 ASP.NET MVC 最早便是一套符合其關係的設計，然而這樣的網路應用程式並不適合規模化後的分工設計制度，進而演進的 ASP.NET Core 便是基於 MVC 的 Controllers 為核心並依循領域驅動為設計概念，因此其中 Views、Models 的替代方案與設計便如下所述：

+ Views
    - [通用用戶端 Web 技術](https://docs.microsoft.com/zh-tw/dotnet/architecture/modern-web-apps-azure/common-client-side-web-technologies)
    - [開發 ASP.NET Core MVC 應用程式](https://docs.microsoft.com/zh-tw/dotnet/architecture/modern-web-apps-azure/develop-asp-net-core-mvc-apps)
+ Models
    - [使用 ASP.NET Core 應用程式中的資料](https://docs.microsoft.com/zh-tw/dotnet/architecture/modern-web-apps-azure/work-with-data-in-asp-net-core-apps)

---

+ [應用程式元件](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/advanced/app-parts?view=aspnetcore-3.1)
+ [應用程式模型](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/application-model?view=aspnetcore-3.1)
