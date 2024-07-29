# Use the official .NET SDK image to build and run the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80

# Copy csproj and restore as distinct layers
COPY ["api.csproj", "./"]
RUN dotnet restore "./api.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "api.csproj" -c Release -o /app/build

# For development, use the base SDK image to run the app
CMD ["dotnet", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:80"]



