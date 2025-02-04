
# SpaceAPI

SpaceAPI is a sample ASP.NET Core Web API that demonstrates how to build a RESTful service using C#. The API manages a collection of "spaces" (a conceptual entity) with various properties such as title, description, and associated projects. It also includes endpoints for file management related to each space. This project is built using the latest (nightly) .NET SDK images, and it is containerized using Docker for easy deployment.

---

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
- [Running the Application](#running-the-application)
  - [Locally](#locally)
  - [Using Docker](#using-docker)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Swagger & API Documentation](#swagger--api-documentation)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

---

## Features

- **CRUD Operations for Spaces:** Create, read, update, and delete spaces.
- **File Management:** Endpoints to add or remove file references from a space.
- **Search Functionality:** Query spaces based on title and description.
- **In-Memory Data Storage:** Uses a static in-memory list to simulate data persistence.
- **CORS Support:** Configured to allow cross-origin requests.
- **Swagger Integration:** Automatically generated API documentation for easy testing.
- **Docker Containerization:** Dockerfile provided for building and running the application in a container.

---

## Project Structure

```
SpaceAPI/
├── Controllers/
│   └── SpacesController.cs       # API endpoints for managing spaces
├── Models/
│   └── Space.cs                  # Space model definition
├── Program.cs                    # Application startup, service configuration, and middleware pipeline
├── Dockerfile                    # Docker configuration to build and run the application
├── SpaceAPI.csproj               # Project file
├── appsettings.json              # Application configuration (e.g., logging)
├── README.md                     # Project documentation (this file)
└── bin/, obj/                   # Build output folders (ignored in version control)
```

---

## Prerequisites

Before you begin, ensure you have met the following requirements:

- **.NET 9.0 SDK (Nightly Build):** This project uses the latest nightly builds of .NET for both SDK and ASP.NET runtime. You can download the latest nightly builds from the [Microsoft .NET website](https://dotnet.microsoft.com/).
- **Docker:** If you plan to run the application in a container, ensure Docker is installed and running on your machine.
- **Git:** For cloning the repository (optional).

---

## Installation & Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/yourusername/SpaceAPI.git
   cd SpaceAPI
   ```

2. **Restore Dependencies:**

   Restore the project dependencies using the .NET CLI:

   ```bash
   dotnet restore
   ```

3. **Build the Project:**

   Compile the project using the following command:

   ```bash
   dotnet build
   ```

4. **Publish the Application:**

   To prepare the application for production (or Docker containerization), publish the project:

   ```bash
   dotnet publish -c Release -o out
   ```

---

## Running the Application

### Locally

You can run the application locally using the .NET CLI:

```bash
dotnet run
```

The application will start, and by default, it listens on ports defined in your configuration (HTTPS redirection is enabled).

### Using Docker

A `Dockerfile` is provided to containerize the application.

1. **Build the Docker Image:**

   The Dockerfile uses the nightly .NET SDK image to build and publish the application, then uses the nightly ASP.NET runtime image to run it.

   ```dockerfile
   # Dockerfile content:
   # ------------------------------------------
   # Use the nightly .NET SDK image to build the app
   FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build-env
   WORKDIR /app

   # Update NO_PROXY so that api.nuget.org bypasses the proxy
   ENV NO_PROXY=api.nuget.org,localhost,intel.com,192.168.0.0/16,172.16.0.0/12,127.0.0.0/8,10.0.0.0/8

   # Copy the csproj and restore as distinct layers
   COPY *.csproj ./
   RUN dotnet restore

   # Copy everything else and build
   COPY . ./
   RUN dotnet publish -c Release -o out

   # Build runtime image
   FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0
   WORKDIR /app
   COPY --from=build-env /app/out .

   # Expose port 80
   EXPOSE 80

   # Set the entry point for the container
   ENTRYPOINT ["dotnet", "SpaceAPI.dll"]
   # ------------------------------------------
   ```

   Build the Docker image with:

   ```bash
   docker build -t spaceapi .
   ```

2. **Run the Docker Container:**

   ```bash
   docker run -d -p 80:80 --name spaceapi-container spaceapi
   ```

   The API should now be accessible at `http://localhost` (or the Docker host IP).

---

## API Endpoints

The `SpacesController` exposes several endpoints for managing spaces. Below is a summary of available endpoints:

- **GET `/api/spaces`**  
  Retrieves all spaces.

- **GET `/api/spaces/{id}`**  
  Retrieves a specific space by its ID.
  
  **Example:**
  ```bash
  GET /api/spaces/1
  ```

- **GET `/api/spaces/search?query={query}`**  
  Searches for spaces by title or description that contain the provided query string.
  
  **Example:**
  ```bash
  GET /api/spaces/search?query=Space
  ```

- **POST `/api/spaces`**  
  Creates a new space. The space details should be sent in the request body as JSON.
  
  **Example:**
  ```json
  {
      "id": 3,
      "title": "New Space",
      "defaultFocusMode": "Focus Mode 3",
      "projects": "Project 3",
      "description": "Description for New Space",
      "private": false,
      "uploadedFiles": "file3.txt",
      "systemMessage": "System message for New Space",
      "groupId": "Group3"
  }
  ```

- **PUT `/api/spaces/{id}`**  
  Updates an existing space by its ID. The updated space object is passed in the request body.
  
- **DELETE `/api/spaces/{id}`**  
  Deletes a space by its ID.

- **POST `/api/spaces/{id}/files`**  
  Adds a file to the space. The file name (or file reference) is passed in the request body.
  
- **DELETE `/api/spaces/{id}/files`**  
  Removes a file from the space. The file name (or file reference) is passed in the request body.

*Note:* The sample implementation uses an in-memory static list to store spaces. This means that data will be lost when the application stops.

---

## Configuration

### Program.cs

The application is configured in `Program.cs` using the .NET minimal hosting model. Key configurations include:

- **Application Insights Telemetry:**  
  Enabled to monitor application performance.

- **Controllers:**  
  Registered using `builder.Services.AddControllers()`.

- **Swagger/OpenAPI:**  
  Swagger services are added for API documentation and testing. Swagger is enabled only in the development environment.

- **CORS Policy:**  
  A CORS policy named `"AllowAll"` is configured to allow requests from any origin, method, and header.

- **HTTPS Redirection:**  
  The application enforces HTTPS redirection.

### appsettings.json

Basic logging configuration is provided in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## Swagger & API Documentation

When running the application in the development environment, Swagger is enabled. You can access the Swagger UI to explore and test the API endpoints:

- **URL:** `http://localhost:<port>/swagger`
- The Swagger UI displays all available endpoints and their expected request/response formats.

---

## Troubleshooting

- **Port Conflicts:**  
  Ensure that port 80 (or the configured port) is not being used by another application on your system. You can modify the exposed port in the Dockerfile or in your launch settings if necessary.

- **Data Persistence:**  
  This sample API uses an in-memory list to store data. If you require persistent storage, consider integrating a database (such as SQL Server, PostgreSQL, etc.) using Entity Framework Core.

- **Nightly Builds:**  
  As this project uses nightly .NET SDK images, be aware that nightly builds may contain unstable features. For production scenarios, it is recommended to use a stable release version of .NET.

---

## Contributing

Contributions are welcome! If you find issues or have suggestions for improvements, please feel free to open an issue or submit a pull request.

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Make your changes and commit them (`git commit -am 'Add some feature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Open a pull request.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

Happy coding!
