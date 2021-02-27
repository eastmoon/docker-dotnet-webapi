# docker-dotnet-webapi

此專案為 .NET Core 伺服器專案，其伺服器架構如下：

+ Server：.NET Core 3.1 WebAPI
+ Database : MySQL 8.0

對應專案目錄設計如下：

```
<project name>
    └ app
    └ db
    └ docker
    └ doc
```

+ app : .NET Core 應用程式子專案
+ db : 資料庫遷移 SQL Schema；遷移使用工具為 dbmate
+ docker : 專案編譯、封裝、測試執行相關虛擬主機容器 Dockerfile
+ doc : 本專案調言與技術說明文件

## 執行專案

+ 操作專案的開發、編譯、封裝指令

```
dockerw.bat [dev | publish | swagger | run | package | db | ef]
```

+ 開發模式

依據 [開發與建置](./doc/dotnet-documnet.md#建置與開發) 調研為基礎，用於啟動並進入 .NET 3.1 SDK 容器，測試開發環境指令

```
dockerw dev
```

+ 發佈專案

依據 [編譯與發佈](./doc/dotnet-documnet.md#編譯與發佈) 調研為基礎，運用 .NET 3.1 SDK 容器發佈專案，其發佈內容會放在 ```cache/published``` 中

```
dockerw publish
```

+ 生成文件

依據 [Open API 文件](./doc/dotnet-documnet.md#Open-API-文件) 調研為基礎，運用 .NET 3.1 SDK 容器生成符合 Swagger & OpenAPI 格式的 JSON 檔案，其生成文件會放在 ```cache/api-doc``` 中

```
dockerw swagger
```

+ 執行發佈內容

依據 [部屬與執行](./doc/dotnet-documnet.md#Open-API-文件) 調研為基礎，用於啟動並進入.NET 3.1 Runtime 容器，測試執行環境指令

```
dockerw run
```

+ 封裝映像檔

依據 [部屬與執行](./doc/dotnet-documnet.md#Open-API-文件) 調研為基礎，運用 .NET 3.1 Runtime 容器位基底，封裝發佈內容並將映像檔匯出，其匯出映像檔會放在 ```cache/package``` 中

```
dockerw package [--run]
```
> --run : 執行封裝完成的映像檔於背景模式，以此確認封裝專案是否可正常執行

+ 啟動資料庫

依據 [Database Migration 範例專案](https://github.com/eastmoon/tutorial-database-dbmate) 為基礎，整合資料庫啟動與遷移工具，達到啟動虛擬資料庫並完成資料庫建置與初始。

```
dockerw db [--down]
```
> --down：關閉當前虛擬資料庫

+ 產生 Entities

依據 [WebService.Entities](./app/WebService.Entities/readme.md) 說明，此腳本是用於啟動開發環境並建置連線虛擬資料庫，以此提供 ```gen-entities.sh``` 腳本正確的資料源。

```
dockerw ef
```
> 若需擬資料庫為開啟，此命令將會因無法連線至正確網路而運作失敗
