# IHC API

This project consists of multiple components that interact with each other to provide a comprehensive API for managing IHC projects.
The main components are:

- **Ihc.Project**: Contains the core models and logic for handling IHC projects.
- **Ihc.Soap**: Implements SOAP-based services for various operations related to IHC projects, including authentication, configuration, and project management.
- **Ihc.WebApi**: Provides a RESTful API for interacting with IHC projects, exposing endpoints for project information, file retrieval, and more.

## Project Structure

- **Ihc.Project**
  - `Model/`: Contains the core models such as `Project`, `Product`, and related classes.
  - `Ihc.Project.csproj`: Project file for Ihc.Project.

- **Ihc.Soap**
  - `Authentication.cs`, `Configuration.cs`, `Controller.cs`, etc.: Implement various SOAP services.
  - `Ihc.Soap.csproj`: Project file for Ihc.Soap.

- **Ihc.WebApi**
  - `Controllers/`: Contains API controllers like `ProjectController`.
  - `Services/`: Contains service classes like `ProjectService`.
  - `Ihc.WebApi.csproj`: Project file for Ihc.WebApi.

## Key Features

- **SOAP Services**: Provides SOAP-based services for operations like project segmentation, subscription management, and more.
- **RESTful API**: Exposes endpoints for project information, file retrieval, and other operations.
- **Core Models**: Defines the core models and logic for handling IHC projects.

# Docker

## Build Docker image

Build Docker image:
> sudo docker build -t local/ihc-api -f Dockerfile .

Test the image:
> sudo docker run -it -p 8080:8080 --rm local/ihc-api

Open in a browser:
http://localhost:8080/swagger/index.html
