#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TodoList/LearningLantern.TodoList/LearningLantern.TodoList.csproj", "TodoList/LearningLantern.TodoList/"]
COPY ["LearningLantern.Common/LearningLantern.Common.csproj", "LearningLantern.Common/"]
RUN dotnet restore "TodoList/LearningLantern.TodoList/LearningLantern.TodoList.csproj"
COPY . .
WORKDIR "/src/TodoList/LearningLantern.TodoList"
RUN dotnet build "LearningLantern.TodoList.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningLantern.TodoList.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningLantern.TodoList.dll"]