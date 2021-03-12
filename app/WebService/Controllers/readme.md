## ASP.NET Core Routes and Controller

對於 Controller 生命週期有以下基本規範 ( 此規範並非正式文件，僅是經由實務、討論得出的結論 )：

+ WebService.Startup.ConfigureServices 中的 services.AddControllers() 會動態建立起所有控制器
    - AddControllers() 的參數物件 [MvcOptions](https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.mvc.mvcoptions?view=aspnetcore-3.1) 是用來針對整個 MVC 框架設定
    - AddControllers().ConfigureApiBehaviorOptions() 的參數物件 [ApiBehaviorOptions](https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.mvc.apibehavioroptions?view=aspnetcore-3.1) 是用來控制 ApiController 屬性的行為狀態開關。
+ Controller 若為 abstract class 則不會被動態建立
+ Controller 若在系統啟動後有確保其類別庫有被參考，則系統會依據 ControllerBase 的登記來建構控制器
    - 目前尚未確認若將控制器宣告在類別庫，但未參考、引用是否仍會被建

ASP.NET Core 是從 ASP.NET MVC 中細分，將 MVC 中的 Controller 分離為 Webapi 專案的核心，因此在相關的設定文獻中仍能看到對 MVC 整體框架的設定細節。

### 路由觀念

在 ASP.NET Core 中設定路由共有兩個方式：

+ [傳統路由](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1#crd)

```
endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```
> 傳統路由是定義在 WebService.Startup.Configure 函式中，此設計是一種典型的路由定義，並指定控制器僅是負責處裡的物件。

+ [屬性路由](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1#ar)

```
public class HomeController : Controller
{
    [Route("")]
    [Route("Home")]
    [Route("Home/Index")]
    [Route("Home/Index/{id?}")]
    public IActionResult Index(int? id)
    {
        return ControllerContext.MyDisplayRouteInfo(id);
    }
}
```
> 屬性路由是將傳統路由的定義改為屬性 ( Attribute ) 定義。

**屬性路由與傳統路由之間的程式設計差異，屬性路由需要更多輸入才能指定路由，傳統路由更簡潔地處理路由設定。不過，屬性路由允許且需要精確地控制要套用至每個動作的路由範本。**

就物件導向程式設計的觀點來說，屬性是一種對類別、行為的特徵宣告，在符合物件導向的設計中，屬性是具有繼承效力，因此屬性路由的設計除了上述官方說法的優點外，在實務上可以透過繼承、多型的方式將共通路由行為抽象化設計，並以不同屬性的設計來擴展系統的架構、共通規範與功能。

### 命名空間路由 ( Namespace Route )

路由名稱的宣告會影響到 API 的路徑名稱，在小型專案中，路由宣告可基於前述來宣告，但隨專案規模擴大，會增加大量 CRUD 功能、特徵功能，這導致區分功能會需依據模組來逐一切分，而這切分會導致在類別撰寫正確定義會有筆誤乃至維護性問題；若考慮前述狀況，並考量維護、擴展性兩點，使用命名空間路由 ( Namespace Route ) 來動態產生路由的前墜是較適當的設計方式。

+ [使用應用程式模型來自訂屬性路由](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1#use-application-model-to-customize-attribute-routes)

官方提供的設計是一個範本，並依此可以看到使用屬性會對控制器有何影響，而這便可以看到同一個屬性物件，可以有兩個用法：

+ 控制器全體

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews(options =>
    {
        options.Conventions.Add(
            new NamespaceRoutingConvention(typeof(Startup).Namespace));
    });
}
```
> 此項宣告是在 WebService.Startup.ConfigureServices 函式中，使用這宣告，則所有控制器皆會被影響；因此範例中迴避已經透過屬性 Route 宣告的j控制器。

+ 單一控制器

```
[NamespaceRoutingConvention("My.Application")]
public class TestController : Controller
{ ... }
```
> 此宣告會基於簡易路由 ( 控制器在未宣告 Route 的情況下，亦未宣告傳統路由，則會基於完整 Namespace 來定義此控制器路由 ) 的運作來觸發 NamespaceRoutingConvention.apply() 來重新定義路由稱呼。

這兩著用法可以併用，亦可獨立使用，即若無在全體宣告，則路由僅會對宣告的控制器有效，因此本專案的範例則是修改其規則併繼承 Route 屬性，以此確保 [ApiController](https://docs.microsoft.com/zh-tw/aspnet/core/web-api/?view=aspnetcore-3.1##apicontroller-attribute) 屬性正常運作。

### 參考文獻

+ [ASP.NET Core 中的路由至控制器動作](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1)
+ [ASP.NET Core 中的路由](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/routing?view=aspnetcore-3.1)
+ [使用 ASP.NET Core 建立 Web API](https://docs.microsoft.com/zh-tw/aspnet/core/web-api/?view=aspnetcore-3.1)
+ [在 ASP.NET Core 中使用應用程式模型](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/application-model?view=aspnetcore-5.0)
