FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["test/GoodREST.Core.Web/GoodREST.Core.Web.csproj", "test/GoodREST.Core.Web/"]
COPY ["test/GoodREST.Core.Test.Services/GoodREST.Core.Test.Services.csproj", "test/GoodREST.Core.Test.Services/"]
COPY ["src/GoodREST.Middleware/GoodREST.Middleware.csproj", "src/GoodREST.Middleware/"]
COPY ["src/GoodREST/GoodREST.csproj", "src/GoodREST/"]
COPY ["test/GoodREST.Core.Test.DataModel/GoodREST.Core.Test.DataModel.csproj", "test/GoodREST.Core.Test.DataModel/"]
COPY ["test/Extensions/GoodREST.Extensions.SwaggerExtension.Tests/GoodREST.Extensions.SwaggerExtension.Tests.csproj", "test/Extensions/GoodREST.Extensions.SwaggerExtension.Tests/"]
COPY ["src/Extensions/GoodREST.Extensions.SwaggerExtension/GoodREST.Extensions.SwaggerExtension.csproj", "src/Extensions/GoodREST.Extensions.SwaggerExtension/"]
COPY ["src/Extensions/GoodREST.Extensions.ServiceDiscovery/GoodREST.Extensions.ServiceDiscovery.csproj", "src/Extensions/GoodREST.Extensions.ServiceDiscovery/"]
COPY ["src/Extensions/GoodREST.Extensions.HealthCheck/GoodREST.Extensions.HealthCheck.csproj", "src/Extensions/GoodREST.Extensions.HealthCheck/"]
RUN dotnet restore "test/GoodREST.Core.Web/GoodREST.Core.Web.csproj"
COPY . .
WORKDIR "/src/test/GoodREST.Core.Web"
RUN dotnet build "GoodREST.Core.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GoodREST.Core.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GoodREST.Core.Web.dll"]