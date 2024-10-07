# TicketMGT.Core.Api

TicketMGT.Core.Api is a RESTful API for managing tickets. It provides endpoints to create, retrieve, update, and delete tickets. The API is built using ASP.NET Core and follows best practices for RESTful services.

## Summary of Endpoints

1. **POST /api/tickets**
   - **Description**: Creates a new ticket.
   - **Request Body**: `Ticket` object.
   - **Responses**:
     - `201 Created`: Returns the newly created ticket.
     - `400 Bad Request`: If the ticket is invalid.
     - `409 Conflict`: If the ticket already exists.
     - `500 Internal Server Error`: If there is a server error.

2. **GET /api/tickets**
   - **Description**: Retrieves all tickets.
   - **Responses**:
     - `200 OK`: Returns the list of tickets.
     - `500 Internal Server Error`: If there is a server error.

3. **GET /api/tickets/{id}**
   - **Description**: Retrieves a ticket by its ID.
   - **Path Parameter**: `id` (GUID) - The ID of the ticket to retrieve.
   - **Responses**:
     - `200 OK`: Returns the ticket.
     - `400 Bad Request`: If the ticket ID is invalid.
     - `404 Not Found`: If the ticket is not found.
     - `500 Internal Server Error`: If there is a server error.

4. **PUT /api/tickets**
   - **Description**: Updates an existing ticket.
   - **Request Body**: `Ticket` object.
   - **Responses**:
     - `200 OK`: Returns the updated ticket.
     - `400 Bad Request`: If the ticket is invalid.
     - `404 Not Found`: If the ticket is not found.
     - `423 Locked`: If the ticket is locked.
     - `500 Internal Server Error`: If there is a server error.

5. **DELETE /api/tickets/{id}**
   - **Description**: Deletes a ticket by its ID.
   - **Path Parameter**: `id` (GUID) - The ID of the ticket to delete.
   - **Responses**:
     - `200 OK`: Returns the deleted ticket.
     - `400 Bad Request`: If the ticket ID is invalid.
     - `404 Not Found`: If the ticket is not found.
     - `423 Locked`: If the ticket is locked.

## Exception Documentation

### TicketValidationException

- **Description**: This exception is thrown when a ticket fails validation.
- **Usage**: Typically used in scenarios where the ticket data does not meet the required validation rules.

### TicketDependencyException

- **Description**: This exception is thrown when there is an issue with a dependency while processing a ticket.
- **Usage**: Used to indicate problems with external dependencies such as databases or other services.

### TicketDependencyValidationException

- **Description**: This exception is thrown when there is a validation issue with a dependency while processing a ticket.
- **Usage**: Used to indicate validation problems with external dependencies.

### TicketServiceException

- **Description**: This exception is thrown when there is a general service error while processing a ticket.
- **Usage**: Used to indicate unexpected errors within the ticket service layer.

### NotFoundTicketException

- **Description**: This exception is thrown when a ticket is not found.
- **Usage**: Used in scenarios where a requested ticket does not exist in the system.

### AlreadyExistsTicketException

- **Description**: This exception is thrown when a ticket already exists.
- **Usage**: Used to indicate that a ticket with the same identifier already exists in the system.

### LockedTicketException

- **Description**: This exception is thrown when a ticket is locked and cannot be modified or deleted.
- **Usage**: Used to indicate that a ticket is currently locked and cannot be processed.

## TicketService Validations

### ValidateTicketOnCreate

- **Description**: Validates the ticket object before creating a new ticket.
- **Parameters**: 
  - `Ticket ticket`: The ticket object to validate.
- **Throws**: 
  - `TicketValidationException`: If the ticket is null or contains invalid data.

### ValidateTicketId

- **Description**: Validates the ticket ID.
- **Parameters**: 
  - `Guid ticketId`: The ID of the ticket to validate.
- **Throws**: 
  - `TicketValidationException`: If the ticket ID is invalid.

### ValidateTicketOnModify

- **Description**: Validates the ticket object before modifying an existing ticket.
- **Parameters**: 
  - `Ticket ticket`: The ticket object to validate.
- **Throws**: 
  - `TicketValidationException`: If the ticket is null or contains invalid data.

### ValidateStorageTicket

- **Description**: Validates the ticket object retrieved from storage.
- **Parameters**: 
  - `Ticket ticket`: The ticket object to validate.
- **Throws**: 
  - `NotFoundTicketException`: If the ticket is not found in storage.

### ValidateStorageTickets

- **Description**: Validates the list of tickets retrieved from storage.
- **Parameters**: 
  - `IQueryable<Ticket> tickets`: The list of tickets to validate.
- **Throws**: 
  - `TicketDependencyException`: If there is an issue with the ticket storage.

### ValidateTicketIsNotNull

- **Description**: Validates that the ticket object is not null.
- **Parameters**: 
  - `Ticket ticket`: The ticket object to validate.
- **Throws**: 
  - `TicketValidationException`: If the ticket is null.

### ValidateTicketIds

- **Description**: Validates the ticket IDs.
- **Parameters**: 
  - `Guid[] ticketIds`: The array of ticket IDs to validate.
- **Throws**: 
  - `TicketValidationException`: If any of the ticket IDs are invalid.
