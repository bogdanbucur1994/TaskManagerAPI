# Use the ASP.NET runtime as the base image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY ["TaskManager.sln", "./"]
COPY ["TaskManager/TaskManager.csproj", "TaskManager/"]
RUN dotnet restore "./TaskManager.sln"

# Copy the entire source code and build the application
COPY . .
WORKDIR "/src/TaskManager"
RUN dotnet build "TaskManager.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "TaskManager.csproj" -c Release -o /app/publish

# Use the runtime image for the final container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.dll"]
