#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .

RUN dotnet restore "./FileOperationsApplication.API/FileOperationsApplication.API.csproj" --disable-parallel
RUN dotnet publish "./FileOperationsApplication.API/FileOperationsApplication.API.csproj" -c release -o /app --no-restore


#Serve Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "FileOperationsApplication.API.dll"]