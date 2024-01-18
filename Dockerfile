# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:latest AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the necessary files to the container
COPY . .

RUN dotnet restore

# Expose the port that the application will run on
EXPOSE 5238

# Build and run the application
CMD ["dotnet", "run"]