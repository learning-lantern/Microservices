FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TextLesson/LearningLantern.TextLesson/LearningLantern.TextLesson.csproj", "TextLesson/LearningLantern.TextLesson/"]
COPY ["Libraries/LearningLantern.BlobStorage/LearningLantern.BlobStorage.csproj", "Libraries/LearningLantern.BlobStorage/"]
COPY ["Libraries/LearningLantern.Common/LearningLantern.Common.csproj", "Libraries/LearningLantern.Common/"]
RUN dotnet restore "TextLesson/LearningLantern.TextLesson/LearningLantern.TextLesson.csproj"
COPY . .
WORKDIR "/src/TextLesson/LearningLantern.TextLesson"
RUN dotnet build "LearningLantern.TextLesson.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningLantern.TextLesson.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningLantern.TextLesson.dll"]
