FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src

COPY MiniHttpServer.sln .
COPY MiniHttpServer.csproj ./

COPY MiniHttpServer.Framework/MiniHttpServer.Framework.csproj MiniHttpServer.Framework/
COPY MiniTemplateEngine/MiniTemplateEngine.csproj MiniTemplateEngine/
COPY MiniTemplateEngine.UnitTests/MiniTemplateEngine.UnitTests.csproj MiniTemplateEngine.UnitTests/
COPY MyORMLibrary/MyORMLibrary.csproj MyORMLibrary/
COPY MyOrmLibrary.UnitTests/MyOrmLibrary.UnitTests.csproj MyOrmLibrary.UnitTests/

RUN dotnet restore

COPY . .

RUN dotnet publish MiniHttpServer.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 54321

# Запуск приложения
ENTRYPOINT ["dotnet", "MiniHttpServer.dll"]