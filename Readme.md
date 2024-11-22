# Clean Architecture Dotnet 9 API Project

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/alex289/CleanArchitecture/dotnet.yml)

This repository contains a sample API project built using the Clean Architecture principles, Onion Architecture, MediatR, and Entity Framework. The project also includes unit tests for all layers and integration tests using xUnit and Nsubstitute.

The purpose of this project is to create a clean boilerplate for an API and to show how to implement specific features.

## Project Structure
The project follows the Onion Architecture, which means that the codebase is organized into layers, with the domain model at the center and the outer layers dependent on the inner layers.

The project has the following structure:

- **Domain**: Contains the domain model, which represents the core business logic of the application. It includes entities, value objects, domain services, and domain events.
- **Application**: Contains the application layer, which implements the use cases of the system. It includes commands, queries, handlers, and DTOs.
- **Infrastructure**: Contains the infrastructure layer, which implements the technical details of the system. It includes database access, logging, configuration, and external services.
- **API**: Contains the presentation layer, which exposes the functionality of the system to the outside world. It includes controllers, action results, and view models.

## Dependencies
The project uses the following dependencies:

- **MediatR**: A lightweight library that provides a mediator pattern implementation for .NET.
- **Entity Framework Core**: A modern object-relational mapper for .NET that provides data access to the application.
- **FluentValidation**: A validation library that provides a fluent API for validating objects.
- **gRPC**: gRPC is an open-source remote procedure call framework that enables efficient communication between distributed systems using a variety of programming languages and protocols.

## Running the Project

To run the project, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution in your IDE of choice.
3. Build the solution to restore the dependencies.
4. Update the connection string in the appsettings.json file to point to your database.
5. Start the API project (Alterntively you can use the `dotnet run --project CleanArchitecture.Api` command)
6. The database migrations will be automatically applied on start-up. If the database does not exist, it will be created.
7. The API should be accessible at `https://localhost:<port>/api/<controller>` where `<port>` is the port number specified in the project properties and `<controller>` is the name of the API controller.

### Using Aspire

1. Run `dotnet run --project CleanArchitecture.AppHost` in the root directory of the project.

### Using docker

Requirements
> This is only needed if running the API locally or only the docker image
1. SqlServer: `docker run --name sqlserver -d -p 1433:1433 -e ACCEPT_EULA=Y -e SA_PASSWORD='Password123!#' mcr.microsoft.com/mssql/server`
1. RabbitMq: `docker run --name rabbitmq -d -p 5672:5672 -p 15672:15672 rabbitmq:4-management`
3. Redis: `docker run --name redis -d -p 6379:6379 -e ALLOW_EMPTY_PASSWORD=yes redis:latest`
4. Add this to the redis configuration in the Program.cs
```csharp
options.ConfigurationOptions = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            EndPoints = { "localhost", "6379" }
        };
```

Running the container
1. Build the Dockerfile: `docker build -t clean-architecture .`
2. Run the Container: `docker run --name clean-architecture -d -p 80:80 -p 8080:8080 clean-architecture`

### Using docker-compose

1. Build the Dockerfile: `docker build -t clean-architecture .`
2. Running the docker compose: `docker-compose up -d` (Delete: `docker-compose down`)

### Using Kubernetes

1. Build the docker image and push it to the docker hub (Change the image name in the `k8s-deployment.yml` to your own)
2. Apply the deployment file: `kubectl apply -f k8s-deployment.yml` (Delete: `kubectl delete -f k8s-deployment.yml`)


## Running the Tests
To run the tests, follow these steps:

1. Open the solution in your IDE of choice.
2. Build the solution to restore the dependencies.
3. Open the Test Explorer window
4. Run all the tests by clicking the Run All button in the Test Explorer.

## Continuous Integration
This project uses GitHub Actions to build and test the project on every commit to the main branch. The workflow consists of several steps, including restoring packages, building the project and running tests.

## Conclusion
This project is a sample implementation of the Clean Architecture principles, Onion Architecture, MediatR, and Entity Framework. It demonstrates how to organize a .NET 9 API project into layers, how to use the MediatR library to implement the mediator pattern, and how to use Entity Framework to access data. It also includes unit tests for all layers and integration tests using xUnit.
