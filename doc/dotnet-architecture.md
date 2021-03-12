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

## ASP.NET 應用程式架構

![ASP.NET Core Clean Architecture](./img/image5-9.png)

![ASP.NET Core Architecture](./img/image5-12.png)

## 應用程式層級

![Application Layers](./img/image5-2.png)

+ [User Interface](../app/WebService/Controller/readme.md)，應用程式介面層，基於 Routes、Controller 物件設計的對外介面。
+ Business Logic，商業邏輯層，實際的介面內服務邏輯與演算法，亦可用 Service Modules、Domain Model 來稱呼。
+ [Data Access](../app/WebService.Entities/readme.md)，資料存取層，將邏輯與演算產生的數據，經由資料存取層回寫至相對應的資料源，基於 Entity Framework Core 物件設計的資料存儲物件。

---

+ [應用程式元件](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/advanced/app-parts?view=aspnetcore-3.1)
+ [應用程式模型](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/application-model?view=aspnetcore-3.1)
