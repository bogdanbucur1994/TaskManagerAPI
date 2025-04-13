# Task Management API (Proof of Concept)

## Purpose
This API serves as a proof of concept (POC) to demonstrate:
- RESTful API design adhering to OpenAPI best practices.
- Effective use of `async/await` for asynchronous programming.
- Abstraction of Service Bus/Queue for flexibility and extensibility.
- Publishing to a queue with a POC for consuming messages (`GetNext` and `GetAll`).
- Real-world standards such as logging, validation, testing, and event handling.

## Setup Instructions

### Prerequisites
- .NET 8 SDK or later installed.
- A code editor like Visual Studio or Visual Studio Code.

### Steps to Run the Application
1. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   cd TaskManager
   ```

2. **Run the Application**:
   - **Using an IDE**:
     - Open the project (`.sln`) in your IDE and use its build/run features.
   - **Using Command Line**:
     - Restore dependencies:
       dotnet restore
     - Run the application:
       dotnet run

3. **Access the Application**:
   - Swagger UI: `http://localhost:5054/index.html`
   - API Base URL: `http://localhost:5054`

## Design Overview

### RESTful API
- Follows REST principles with proper HTTP methods (`GET`, `POST`, `PATCH`, `DELETE`).
- Uses meaningful HTTP status codes (e.g., `200 OK`, `201 Created`, `404 Not Found`).
- Designed with idempotence in mind.

### Service Bus/Queue Abstraction
- The `IMessageQueue` interface abstracts the message queue implementation.
- An in-memory queue (`InMemoryQueue`) is used for simplicity in this POC.
- The `MessageBus` class provides a higher-level abstraction over the queue.

### Asynchronous Programming
- All service and repository methods use `async/await` for non-blocking operations.

### Layered Architecture
- **Controllers**: Handle HTTP requests and responses.
- **Services**: Contain business logic.
- **Repositories**: Manage data access using EF Core with an in-memory database.

### Additional Features
- Event handling for `TaskCreated` and `TaskUpdated`.
- File-based logging.
- Basic validation implemented directly in models.

## Trade-offs and Limitations
1. **Lack of DTOs**:
   - Domain models are directly used in requests and responses for simplicity.
2. **No DDD (Domain-Driven Design)**:
   - The design avoids strict DDD principles to keep the implementation lightweight.
3. **Simplified Folder Structure**:
   - All layers are organized within a single project for ease of setup.
4. **In-Memory Queue**:
   - The `InMemoryQueue` is a mock implementation and should be replaced with a production-ready message queue (e.g., RabbitMQ, Azure Service Bus).
5. **In-Memory Database**:
   - EF Core is configured with an in-memory database, which should be replaced with a persistent database (e.g., SQL Server) for real-world use cases.
6. **Testing**:
   - Some tests are implemented using Moq for functional and behavioral testing but are incomplete (`TODO`).
7. **DOCKER file generated but not tested**
   - A simple DOCKER file was added but no time left to install Docker and test - future improvement
8. **NO AUTH mecanism yet**

## Future Improvements
- Introduce DTOs for better separation of concerns between API and domain models.
- Replace the in-memory database and queue with production-ready implementations.
- Refactor into a multi-project solution for improved modularity and scalability.
- Adopt DDD principles for a more robust domain model.
- Expand unit test coverage.
- Enhance test reliability and fix existing issues.
- Install Docker and DOCKERize using Dockerfile
