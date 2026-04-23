# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy versioning and build configuration files
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

# Copy csproj files and restore dependencies
COPY ["src/LesReken.Web/LesReken.Web.csproj", "src/LesReken.Web/"]
COPY ["src/LesReken.Web.Client/LesReken.Web.Client.csproj", "src/LesReken.Web.Client/"]
COPY ["src/LesReken.Application/LesReken.Application.csproj", "src/LesReken.Application/"]
COPY ["src/LesReken.Domain/LesReken.Domain.csproj", "src/LesReken.Domain/"]
COPY ["src/LesReken.Infrastructure/LesReken.Infrastructure.csproj", "src/LesReken.Infrastructure/"]
COPY ["src/LesReken.ServiceDefaults/LesReken.ServiceDefaults.csproj", "src/LesReken.ServiceDefaults/"]

RUN dotnet restore "src/LesReken.Web/LesReken.Web.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish
WORKDIR "/src/src/LesReken.Web"
RUN dotnet publish "LesReken.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Environment setup for SQLite data persistence
RUN mkdir -p /app/data
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/lesreken.db"

# Expose port 8080 (ASP.NET Core 10 uses 8080 by default)
EXPOSE 8080

ENTRYPOINT ["dotnet", "LesReken.Web.dll"]
