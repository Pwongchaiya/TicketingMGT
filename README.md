          # TicketMGT.Core.Api

Table of Contents
- [Overview](#overview)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Ticket Model](#ticket-model)
  - [Properties](#properties)
  - [Example JSON Representation](#example-json-representation)
  - [Enums](#enums)
- [Summary of Endpoints](#summary-of-endpoints)
  - [POST /api/tickets](#post-apitickets)
  - [GET /api/tickets](#get-apitickets)
  - [GET /api/tickets/{id}](#get-apiticketsid)
  - [PUT /api/tickets](#put-apitickets)
  - [DELETE /api/tickets/{id}](#delete-apiticketsid)
- [Exception Documentation](#exception-documentation)
- [TicketService Validations](#ticketservice-validations)

## Overview

This API allows clients to manage tickets through a set of well-defined endpoints. Each endpoint is designed to handle specific operations related to ticket management, such as creating a new ticket, retrieving existing tickets, updating ticket information, and deleting tickets.

## Getting Started

### Prerequisites

- .NET Core SDK
- ASP.NET Core
- A tool to test API endpoints (e.g., Postman, curl)

### Installation

1. Clone the repository: `git clone https://github.com/your-repo/TicketMGT.Core.Api.git`
2. Navigate to the project directory: `cd TicketMGT.Core.Api`
3. Restore the dependencies: `dotnet restore`
4. Run the application: `dotnet run`

## Ticket Model

The `Ticket` model represents a ticket in the TicketMGT.Core.Api system. It contains the following properties:

### Properties

- **Id** (GUID)
  - **Description**: The unique identifier for the ticket.
  - **Example**: `d290f1ee-6c54-4b01-90e6-d701748f0851`

- **Title** (string)
  - **Description**: The title of the ticket.
  - **Example**: `"Bug in login feature"`

- **Description** (string)
  - **Description**: A detailed description of the ticket.
  - **Example**: `"Users are unable to log in using their credentials"`

- **Status** (TicketStatus)
  - **Description**: The current status of the ticket.
  - **Enum Values**: `New`, `InProgress`, `Completed`, `Archived`
  - **Example**: `New`

- **Priority** (TicketPriority)
  - **Description**: The priority level of the ticket.
  - **Enum Values**: `Low`, `Medium`, `High`
  - **Example**: `High`

- **CreatedDate** (DateTimeOffset)
  - **Description**: The date and time when the ticket was created.
  - **Example**: `2023-10-01T12:34:56Z`

- **UpdatedDate** (DateTimeOffset)
  - **Description**: The date and time when the ticket was last updated.
  - **Example**: `2023-10-02T14:20:00Z`

- **CompletedAt** (DateTimeOffset?)
  - **Description**: The date and time when the ticket was completed.
  - **Example**: `2023-10-03T16:45:00Z`

- **DueDate** (DateTimeOffset?)
  - **Description**: The due date and time for the ticket.
  - **Example**: `2023-10-05T23:59:59Z`

- **AssignedToUserId** (GUID?)
  - **Description**: The ID of the user to whom the ticket is assigned.
  - **Example**: `a1234567-89ab-cdef-0123-456789abcdef`

- **CreatedByUserId** (GUID?)
  - **Description**: The ID of the user who created the ticket.
  - **Example**: `b2345678-90bc-def0-1234-567890abcdef`

- **IsRecurring** (bool)
  - **Description**: Indicates whether the ticket is recurring.
  - **Example**: `true`

- **RecurrencePattern** (RecurrencePattern?)
  - **Description**: The recurrence pattern for the ticket.
  - **Example**: `Daily`

- **IsNotificationEnabled** (bool)
  - **Description**: Indicates whether notifications are enabled for the ticket.
  - **Example**: `true`

- **ReminderDate** (DateTimeOffset?)
  - **Description**: The date and time for the reminder notification.
  - **Example**: `2023-10-04T09:00:00Z`

- **EstimatedTimeToCompleteInHours** (long?)
  - **Description**: The estimated time to complete the ticket in hours.
  - **Example**: `4`

- **ActualTimeToCompleteInHours** (long?)
  - **Description**: The actual time taken to complete the ticket in hours.
  - **Example**: `3`

### Example JSON Representation
### Enums

#### TicketPriority

- **Low**
- **Medium**
- **High**

#### TicketStatus

- **New**
- **InProgress**
- **Completed**
- **Archived**

## Summary of Endpoints

### POST /api/tickets
- **Description**: Creates a new ticket.
- **Request Body**: A JSON object representing the `Ticket`.
- **Responses**:
  - `201 Created`: Returns the newly created ticket.
  - `400 Bad Request`: If the ticket is invalid.
  - `409 Conflict`: If the ticket already exists.
  - `500 Internal Server Error`: If there is a server error.

### GET /api/tickets
- **Description**: Retrieves all tickets.
- **Responses**:
  - `200 OK`: Returns the list of tickets.
  - `500 Internal Server Error`: If there is a server error.

### GET /api/tickets/{id}
- **Description**: Retrieves a ticket by its ID.
- **Path Parameter**: `id` (GUID) - The ID of the ticket to retrieve.
- **Responses**:
  - `200 OK`: Returns the ticket.
  - `400 Bad Request`: If the ticket ID is invalid.
  - `404 Not Found`: If the ticket is not found.
  - `500 Internal Server Error`: If there is a server error.

### PUT /api/tickets
- **Description**: Updates an existing ticket.
- **Request Body**: A JSON object representing the `Ticket`.
- **Responses**:
  - `200 OK`: Returns the updated ticket.
  - `400 Bad Request`: If the ticket is invalid.
  - `404 Not Found`: If the ticket is not found.
  - `423 Locked`: If the ticket is locked.
  - `500 Internal Server Error`: If there is a server error.

### DELETE /api/tickets/{id}
- **Description**: Deletes a ticket by its ID.
- **Path Parameter**: `id` (GUID) - The ID of the ticket to delete.
- **Responses**:
  - `200 OK`: Returns the deleted ticket.
  - `400 Bad Request`: If the ticket ID is invalid.
  - `404 Not Found`: If the ticket is not found.
  - `423 Locked`: If the ticket is locked.

## Exception Documentation

### TicketValidationException
- **Description**: Thrown when a ticket fails validation.
- **Usage**: When ticket data does not meet validation rules.

### TicketDependencyException
- **Description**: Thrown when there is an issue with a dependency while processing a ticket.
- **Usage**: Indicates problems with external dependencies like databases.

### TicketDependencyValidationException
- **Description**: Thrown when there is a validation issue with a dependency while processing a ticket.
- **Usage**: Indicates validation problems with external dependencies.

### TicketServiceException
- **Description**: Thrown when there is a general service error while processing a ticket.
- **Usage**: Indicates unexpected errors within the ticket service layer.

### NotFoundTicketException
- **Description**: Thrown when a ticket is not found.
- **Usage**: When a requested ticket does not exist in the system.

### AlreadyExistsTicketException
- **Description**: Thrown when a ticket already exists.
- **Usage**: Indicates that a ticket with the same identifier already exists.

### LockedTicketException
- **Description**: Thrown when a ticket is locked and cannot be modified or deleted.
- **Usage**: Indicates that a ticket is currently locked and cannot be processed.

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
