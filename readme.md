# docker-dotnet-webapi

此專案為 .NET Core 伺服器專案，其伺服器架構如下：

+ Server：.NET Core 5.0 WebAPI
+ Database : MySQL 8.0

以下為專案建立

```
mkdir <project name>
cd <project name>
mkdir app
mkdir db
```

## 執行專案

## .NET Core

此段記錄專案基礎建立方式，僅供參考。

#### 建置與開發

+ 下載環境

```
docker pull mcr.microsoft.com/dotnet/sdk:5.0
```
> 開發環境使用 .NET Core 5.0，因此可至 [.NET SDK](https://hub.docker.com/_/microsoft-dotnet-sdk) 下載開發容器

+ 進入環境

```
docker run -ti -v %cd%\app:/repo mcr.microsoft.com/dotnet/sdk:5.0 bash
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
dotnet new classlib --no-restore -f net5.0 -o WebService.Core
dotnet sln add $(ls -r **/*.csproj)
```
> 以上操作可透過 Visual Studio IDE 建立，本段考慮使用命令腳本是為確認期間關聯與進行維護所需工具確認，在實務設計與開發仍會建議開發人員使用 IDE 來加速開發。

#### 編譯與發佈

+ 進入環境

編譯所需的環境與開發一致。

```
docker run -ti -v %cd%\app:/repo -v %cd%\cache\published:/repo/published mcr.microsoft.com/dotnet/sdk:5.0 bash
```
> 重新進入開發環境，並加上發佈目錄

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
docker pull mcr.microsoft.com/dotnet/aspnet:5.0
```
> 部屬環境使用 ASP.NET Core 5.0 Runtime 將編譯的發佈內容封裝，因此可至 [.NET Aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet) 下載開發容器

+ 進入環境

```
docker run -ti -v %cd%\cache\published:/repo -p 5000:80 mcr.microsoft.com/dotnet/aspnet:5.0 bash
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
