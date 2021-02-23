# docker-dotnet-webapi

此專案為 .NET Core 伺服器專案，其伺服器架構如下：

+ Server：.NET Core 3.1 WebAPI
+ Database : MySQL 8.0

以下為專案建立

```
mkdir <project name>
cd <project name>
mkdir app
mkdir db
```

## 執行專案

+ 操作專案的開發、編譯、封裝指令

```
dockerw.bat [dev | publish | swagger | run | package | db | ef]
```

+ 開發模式

進入 .NET 3.1 SDK 容器

```
dockerw dev
```

+ 發佈專案

依據 .NET 3.1 SDK 容器發佈專案，其發佈內容會放在 ```cache/published``` 中

```
dockerw publish
```

+ 生成文件

依據 .NET 3.1 SDK 容器生成符合 Swagger & OpenAPI 格式的 JSON 檔案，其生成文件會放在 ```cache/api-doc``` 中

```
dockerw swagger
```

+ 執行發佈內容

依據 .NET 3.1 Runtime 容器，執行發佈完成的內容

```
dockerw run
```

+ 封裝映像檔

依據 .NET 3.1 Runtime 容器，封裝發佈內容並將映像檔匯出，其匯出映像檔會放在 ```cache/package``` 中

```
dockerw package [--run]
```
> --run : 執行封裝完成的映像檔於背景模式，以此確認封裝專案是否可正常執行

+ 啟動資料庫

依據 [Database Migration 範例](https://github.com/eastmoon/tutorial-database-dbmate) 為基礎，整合資料庫啟動與遷移工具，達到啟動虛擬資料庫並完成資料庫建置與初始。

```
dockerw db [--down]
```
> --down：關閉當前虛擬資料庫

+ 產生 Entities

依據 [app/WebService.Entities](./app/WebService.Entities/readme.md) 說明，此腳本是用於啟動開發環境並建置連線虛擬資料庫，以此提供 ```gen-entities.sh``` 腳本正確的資料源。

```
dockerw ef
```
> 若需擬資料庫為開啟，此命令將會因無法連線至正確網路而運作失敗

## .NET Core

此段記錄專案基礎建立方式，僅供參考。

#### 建置與開發

+ 下載環境

```
docker pull mcr.microsoft.com/dotnet/sdk:3.1
```
> 開發環境使用 .NET Core 3.1，因此可至 [.NET SDK](https://hub.docker.com/_/microsoft-dotnet-sdk) 下載開發容器

+ 進入環境

```
docker run -ti -v %cd%\app:/repo mcr.microsoft.com/dotnet/sdk:3.1 bash
```

+ 建立專案
    - [dotnet 命令](https://docs.microsoft.com/zh-tw/dotnet/core/tools/dotnet)
    - [dotent new](https://docs.microsoft.com/zh-tw/dotnet/core/tools/dotnet-new)
    - [dotent sln](https://docs.microsoft.com/zh-tw/dotnet/core/tools/dotnet-sln)
    - [.gitattributes template](https://gitattributes.io/)

```
cd /repo
dotnet new sln
dotnet new gitignore
curl -o .gitattribures https://gitattributes.io/api/visualstudio
dotnet new webapi --no-restore -o WebService
dotnet new classlib --no-restore -f netcoreapp3.1 -o WebService.Core
dotnet sln add $(ls -r **/*.csproj)
```
> 以上操作可透過 Visual Studio IDE 建立，本段考慮使用命令腳本是為確認期間關聯與進行維護所需工具確認，在實務設計與開發仍會建議開發人員使用 IDE 來加速開發。

#### 編譯與發佈

+ 進入環境

編譯所需的環境與開發一致，但需增加正確的輸出目錄。

```
docker run -ti -v %cd%\app:/repo -v %cd%\cache\published:/repo/published mcr.microsoft.com/dotnet/sdk:3.1 bash
```

+ 發佈專案

```
cd /repo
rm -rf published/*
dotnet publish --configuration Release -o published
```
> 前往專案目錄並執行編譯指令，若要編譯專案則參考 ```dotnet build``` 指令[文獻](https://docs.microsoft.com/zh-tw/dotnet/core/tools/dotnet-build)

#### 部屬與執行

+ 下載環境

```
docker pull mcr.microsoft.com/dotnet/aspnet:3.1
```
> 部屬環境使用 ASP.NET Core 3.1 Runtime 將編譯的發佈內容封裝，因此可至 [.NET Aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet) 下載開發容器

+ 進入環境

```
docker run -ti -v %cd%\cache\published:/repo -p 5000:80 mcr.microsoft.com/dotnet/aspnet:3.1 bash
```

+ 執行專案

```
cd /repo
dotnet WebService.dll
```

+ 測試執行

```
curl http://localhost:5000/WeatherForecast
```

#### 產生 API 文件

使用 Visual Studio 測試時，會直接產生 Swagger 頁面，但其相關文件若需編譯時產生則需使用額外工具產生。

[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md) 是 .NET Core 專案中用於產生 Swagger 與 OpenAPI 文件的套件，其中 [Swashbuckle.AspNetCore.Cli](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#swashbuckleaspnetcorecli) 則是另 .NET 命令介面啟用 ```dotnet swagger``` 來輸出指令。


+ 使用此套件有以下需注意事項：
    - 套件版本需注意，在實際使用 cli 套件後會發現 .NET 3.1 必須使用 6.0.7，其 5.6.3 使用版本上限為 ASP.NET Core 3.0
    - 空白專案需使用 ```dotnet new tool-manifest``` 產生 ```.config``` 目錄與相關文件檔
    - 編譯容器並未預設安裝 ```Swashbuckle.AspNetCore.Cli```，可於產生專案容器時先行安裝至 global 後，再由 ```dotnet restore``` 恢復套件使用
    - 產生文件前需透過發佈產生二進制動態函式庫連結檔

+ 進入環境

文件產生所需的環境與開發一致，但需增加正確的輸出目錄。

```
docker run -ti -v %cd%\app:/repo -v %cd%\cache\published:/repo/published -v %cd%\cache\api-doc:/repo/api-doc mcr.microsoft.com/dotnet/sdk:3.1 bash
```

+ 掛載工具

```
dotnet tool install -g swashbuckle.aspnetcore.cli --version 6.0.7
swagger --help
```
> 全域環境

```
dotnet new tool-manifest --force
dotnet tool install swashbuckle.aspnetcore.cli --version 6.0.7
dotnet swagger
```
> 區域環境

+ 產生文件

```
dotnet swagger tofile --output ./api-doc/swagger.json ./published/WebService.dll v1
```
> 文件產生工具參數說明
> + [output] : .json 檔案輸出位置與檔名
> + [startupassembly]  : 文件檔案產生源頭的動態函式庫連結檔
> + [swaggerdoc] : 在動態函式庫連結檔中定義的  swagger doc 標籤；在 Startup.cs 中 ```c.SwaggerDoc``` 所設定的標籤變數，若有版本改動，輸出文檔也會改動
