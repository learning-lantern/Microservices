#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Calendar/LearningLantern.Calendar/LearningLantern.Calendar.csproj", "Calendar/LearningLantern.Calendar/"]
COPY ["LearningLantern.Common/LearningLantern.Common.csproj", "LearningLantern.Common/"]
RUN dotnet restore "Calendar/LearningLantern.Calendar/LearningLantern.Calendar.csproj"
COPY . .
WORKDIR "/src/Calendar/LearningLantern.Calendar"
RUN dotnet build "LearningLantern.Calendar.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningLantern.Calendar.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningLantern.Calendar.dll"]