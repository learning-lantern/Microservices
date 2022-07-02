FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Video/LearningLantern.Video/LearningLantern.Video.csproj", "Video/LearningLantern.Video/"]
COPY ["Libraries/LearningLantern.BlobStorage/LearningLantern.BlobStorage.csproj", "Libraries/LearningLantern.BlobStorage/"]
COPY ["Libraries/LearningLantern.Common/LearningLantern.Common.csproj", "Libraries/LearningLantern.Common/"]
RUN dotnet restore "Video/LearningLantern.Video/LearningLantern.Video.csproj"
COPY . .
WORKDIR "/src/Video/LearningLantern.Video"
RUN dotnet build "LearningLantern.Video.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningLantern.Video.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningLantern.Video.dll"]
