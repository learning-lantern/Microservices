FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Admin/LearningLantern.Admin/LearningLantern.Admin.csproj", "Admin/LearningLantern.Admin/"]
COPY ["ApiGateway/LearningLantern.ApiGateway/LearningLantern.ApiGateway.csproj", "ApiGateway/LearningLantern.ApiGateway/"]
COPY ["Libraries/LearningLantern.Common/LearningLantern.Common.csproj", "Libraries/LearningLantern.Common/"]
COPY ["Libraries/LearningLantern.EventBus/LearningLantern.EventBus.csproj", "Libraries/LearningLantern.EventBus/"]
RUN dotnet restore "Admin/LearningLantern.Admin/LearningLantern.Admin.csproj"
COPY . .
WORKDIR "/src/Admin/LearningLantern.Admin"
RUN dotnet build "LearningLantern.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningLantern.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningLantern.Admin.dll"]
