# Use dotnet SDK image to build, use dotnet runtime image to run app?
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


# Copy csproj which stores dependencies, and restore them
COPY *.csproj .

RUN dotnet restore TeamApp.csproj

# Copy the rest and build
COPY . .
RUN dotnet publish TeamApp.csproj -property:AssemblyName=TeamApp -o /app/out

# Move to runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Default http port?
EXPOSE 80

# Entrypoint
ENTRYPOINT ["dotnet", "TeamApp.dll"]
