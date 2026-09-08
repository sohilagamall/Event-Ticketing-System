# Event Ticketing API

A backend system for managing events, ticket sales, reservations, and event access.

The project is designed around a real-world event ticketing workflow, where users can discover events, reserve and purchase tickets, while organizers manage their events and administrators manage the system.

## Overview

The system supports three main types of users:

* **Customers** — browse events, reserve and purchase tickets, and manage their tickets.
* **Organizers** — create and manage their events, ticket types, and sales.
* **Admins** — manage users and oversee the platform.

The project also focuses on backend problems that appear in real ticketing systems, such as ticket availability, concurrent reservations, reservation expiration, payments, idempotency, and ticket validation.

## Main Features

### Authentication & Authorization

* User registration and login
* JWT-based authentication
* Role-based authorization
* Support for multiple roles per user
* Resource ownership checks

### Event Management

* Create and manage events
* Event lifecycle management
* Publish and cancel events
* Manage event details and availability
* Event discovery with filtering, searching, sorting, and pagination

### Ticket Management

* Create different ticket types for an event
* Set ticket prices and quantities
* Track ticket availability
* Prevent selling more tickets than available

### Reservations & Orders

* Temporarily reserve tickets
* Automatically expire reservations
* Release expired ticket quantities
* Create orders from successful reservations
* Handle concurrent purchase attempts safely

### Payments

* Integrate payment processing
* Handle payment success and failure
* Use idempotency to prevent duplicate payment/order operations

### Tickets

* Generate tickets after a successful purchase
* Associate tickets with their owners and events
* Validate tickets at event entry
* Prevent the same ticket from being used more than once

### Background Processing

* Process expired reservations
* Handle other tasks that should run outside the request lifecycle

## User Roles

| Role      | Responsibilities                                                     |
| --------- | -------------------------------------------------------------------- |
| Customer  | Browse events, reserve tickets, purchase tickets, and manage tickets |
| Organizer | Create and manage events and ticket types                            |
| Admin     | Manage users and oversee the system                                  |

A user can have more than one role. For example, an organizer can also use the platform as a customer.

## Architecture

The project follows a layered architecture with clear separation between the API, application logic, domain, and infrastructure.

```text
EventTicketing
│
├── src
│   ├── EventTicketing.Api
│   ├── EventTicketing.Application
│   ├── EventTicketing.Domain
│   └── EventTicketing.Infrastructure
│
└── tests
    ├── EventTicketing.UnitTests
    └── EventTicketing.IntegrationTests
```

### API

Responsible for handling HTTP requests, authentication configuration, authorization policies, controllers, and middleware.

### Application

Contains the application's use cases and business workflows. It defines the interfaces and logic needed by the application without depending on infrastructure implementations.

### Domain

Contains the core business concepts and rules of the ticketing system.

The domain layer does not depend on the API, database, or external services.

### Infrastructure

Contains implementations that interact with external systems, including:

* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT token generation
* Payment services
* Background processing
* Other external integrations

## Technologies

* **C#**
* **.NET 9**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Core Identity**
* **JWT**
* **Swagger / OpenAPI**
* **xUnit** for testing

## Backend Concepts

This project is also used to practice and apply real backend engineering concepts, including:

* Authentication and authorization
* Role-based and resource-based authorization
* Pagination, filtering, searching, and sorting
* Database relationships and transactions
* Concurrency control
* Race-condition prevention
* Reservation expiration
* Idempotent APIs
* Background processing
* Payment workflows
* Ticket validation
* Unit and integration testing
* Caching and performance considerations

## Project Status

The project is being developed incrementally.

### Completed

* Authentication
* JWT authorization
* User roles
* Role-based authorization
* User management
* Pagination
* Filtering
* Searching
* Sorting

### Planned

* Event management
* Ticket types and inventory
* Ticket reservations
* Reservation expiration
* Order management
* Payment integration
* Ticket generation and validation
* Background processing
* Notifications
* Caching
* Expanded unit and integration testing

## Project Goals

The main goal of this project is to build a realistic backend system while understanding the engineering decisions behind it.

Rather than adding patterns or technologies for their own sake, each part of the system is introduced when it solves an actual problem in the application.
