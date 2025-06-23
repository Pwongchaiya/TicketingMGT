# Codebase Overview

This project contains an ASP.NET Core Web API for managing tickets. The solution uses Entity Framework Core for data access and is structured using brokers, services, and controllers.

## Solution Structure
- **TicketMGT.Core.Api** – main Web API project.
- **TicketMGT.Core.Api.Infrastructure.Build** – build scripts and configurations.
- **TicketMGT.Core.Api.Tests.Acceptance** – end‑to‑end tests for HTTP endpoints.
- **TicketingMGT.Core.Api.Tests.Unit** – unit tests for the service layer.

## Architecture
The API registers its services and brokers inside `Program.cs`:
```csharp
builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
builder.Services.AddTransient<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<ITicketService, TicketService>();
```
【F:TicketMGT.Core.Api/Program.cs†L34-L38】

`TicketController` exposes CRUD endpoints and relies on `ITicketService`:
```csharp
[ApiController]
[Route("api/[controller]")]
public class TicketController : RESTFulController
```
【F:TicketMGT.Core.Api/Controllers/TicketController.cs†L12-L15】

`TicketService` orchestrates validation and storage operations via the brokers:
```csharp
public partial class TicketService : ITicketService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;
    private readonly IDateTimeBroker dateTimeBroker;
```
【F:TicketMGT.Core.Api/Services/Foundations/TicketService.cs†L11-L18】

The ticket entity contains various properties to track status, priority and scheduling information:
```csharp
public class Ticket
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    // ...additional properties...
}
```
【F:TicketMGT.Core.Api/Models/Foundations/Tickets/Ticket.cs†L5-L23】

## Running the Application
1. Install the .NET SDK (version 8 or higher).
2. Restore dependencies and run the API:
   ```bash
   dotnet restore
   dotnet run
   ```
   from the `TicketMGT.Core.Api` directory.

## Testing
Both unit and acceptance tests can be executed with:
```bash
dotnet test
```
The GitHub workflow `.github/workflows/dotnet.yml` also performs build and test on each pull request.
【F:.github/workflows/dotnet.yml†L1-L35】
