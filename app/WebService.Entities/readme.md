## Entity Framework Core

Entity Framework (EF) Core 是常見 Entity Framework 資料存取技術，並作為物件關聯式對應程式 (O/RM) 的通訊與資料物件，相關說明可參考下列文件。

+ [Entity Framework Core](https://docs.microsoft.com/zh-tw/ef/core/)
+ [開始使用 EF Core](https://docs.microsoft.com/zh-tw/ef/core/get-started/overview/first-app?tabs=netcore-cli)
+ [Entity Framework Core 工具參考-.NET Core CLI](https://docs.microsoft.com/zh-tw/ef/core/cli/dotnet)

在實驗與實務上，由於資料庫的複雜性，若採用人工編寫 Entity 容易導致設定面錯誤，因此當資料庫逐漸擴大則建議採用工具動態生成，然而考量此物件的變動性，生成程序應在軟體編譯與部屬前先行透過工具生成，避免將此類動態生成物件上傳至版本控制；也正因為動態生成的原始碼不會上傳，需注意若是人工編寫的內容，則應分屬在不同資料夾或應透過生成腳本動態加入程式碼 ```gen-entities.sh```。

**需注意，由於專案架構設計關係，在框架啟動時會於 ```WebService.Infrastructure.Extensions``` 中設定 DBContext 物件的連線資訊，若在此階段前為完成生成則勢必導致編譯異常。**

#### 環境設定

WebService.Entitites 物件受 WebService.Core 參考，並透過 Core 使用於核心通訊層級與相關參考 Core 的服務模組；因此，為用於動態生成與運作正常，該專案至少擁有兩個必須套件 ( 透過 NuGet 設定 )

+ Database Connection Package
    - 此範例使用 Pomelo.EntityFrameworkCore.MySql
+ Microsoft.EntityFrameworkCore.Design
    - 對應專案 Framework 設定的 SDK 版本，若使用高於此版本的內容會導致 ```dontnet ef``` 指令失敗

#### 腳本設定

本專案的動態生成考量編譯環境與適用環境，使用 Docker 並執行 .NET 3.1 SDK 環境，此環境應於 Dockerfile 完成工具安裝

```
dotnet tool install -g dotnet-ef
```

生成腳本則撰寫於 ```gen-entities.sh``` 檔案中。

**需注意，此生成腳本預設連線 SQL 主機是同虛擬網路內的對象主機，若未能在同虛擬網路內掛上 MySQL 主機則會導致連線異常。**
