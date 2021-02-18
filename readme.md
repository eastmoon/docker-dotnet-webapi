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

+ 編譯環境

```
docker pull mcr.microsoft.com/dotnet/sdk:5.0
```
> 開發環境使用 .NET Core 5.0，因此可至 [.NET SDK](https://hub.docker.com/_/microsoft-dotnet-sdk) 下載開發容器

+ 進入開發環境

```
docker run -ti -v %cd%:/repo mcr.microsoft.com/dotnet/sdk:5.0 bash
cd /repo/app
```

+ 建立專案
    - [dotnet 命令](https://docs.microsoft.com/zh-tw/dotnet/core/tools/dotnet)
    - [dotent new](https://docs.microsoft.com/zh-tw/dotnet/core/tools/dotnet-new)
    - [dotent sln](https://docs.microsoft.com/zh-tw/dotnet/core/tools/dotnet-sln)
    - [.gitattributes template](https://gitattributes.io/)

```
dotnet new sln
dotnet new gitignore
curl -o .gitattribures https://gitattributes.io/api/visualstudio
dotnet new webapi --no-restore -o WebService
dotnet new classlib --no-restore -f net5.0 -o WebService.Core
dotnet sln add $(ls -r **/*.csproj)
```
> 以上操作可透過 Visual Studio IDE 建立，本段考慮使用命令腳本是為確認期間關聯為主，在實務設計與開發仍會建議開發人員使用 IDE 來加速開發。
