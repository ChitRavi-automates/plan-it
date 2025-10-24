# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project file(s) and restore dependencies
COPY backend/*.csproj ./backend/
RUN dotnet restore backend/

# Copy all source code
COPY backend/. ./backend/

# Build and publish the app in Release mode to /app/out
RUN dotnet publish backend/ -c Release -o /app/out

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/out ./

# Expose port 80 for the container
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "plan-it.dll"]
