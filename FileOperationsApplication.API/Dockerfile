#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FileOperationsApplication.API/FileOperationsApplication.API.csproj", "FileOperationsApplication.API/"]
RUN dotnet restore "FileOperationsApplication.API/FileOperationsApplication.API.csproj"
COPY . .
WORKDIR "/src/FileOperationsApplication.API"
RUN dotnet build "FileOperationsApplication.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileOperationsApplication.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileOperationsApplication.API.dll"]