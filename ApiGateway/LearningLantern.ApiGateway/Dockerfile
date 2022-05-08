#See http://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateway/LearningLantern.ApiGateway/LearningLantern.ApiGateway.csproj", "ApiGateway/LearningLantern.ApiGateway/"]
COPY ["LearningLantern.Common/LearningLantern.Common.csproj", "LearningLantern.Common/"]
RUN dotnet restore "ApiGateway/LearningLantern.ApiGateway/LearningLantern.ApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateway/LearningLantern.ApiGateway"
RUN dotnet build "LearningLantern.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningLantern.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningLantern.ApiGateway.dll"]