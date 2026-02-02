# DinnerSpinner

DinnerSpinner is a small, end-to-end meal-planning application built with **ASP.NET Core** and **Blazor WebAssembly**. It helps households keep a list of home-cooked dishes and randomly “spin” to decide what to eat for the next meal or several days.

While intentionally modest in scope, this project is designed as a **portfolio showcase** demonstrating clean API design, vertical slice architecture, and the practical application of **Domain-Driven Design (DDD)** principles. Although DDD is often associated with larger systems, its core ideas, such as **explicit boundaries**, **rich domain models**, and **enforced invariants**, remain valuable even in small applications.

* * *

## Goals & design rationale

DinnerSpinner is intentionally small, but built to demonstrate how I design real systems: clear boundaries, explicit contracts, and domain invariants enforced in the right place using **Domain-Driven Design**.

The goal is not architectural complexity for its own sake, but clarity of intent and correctness of behavior.

* * *

## Domain-Driven Design approach

This project explicitly applies **DDD principles**:

* A **domain layer** that models the core concepts of the problem space (Dishes, Categories, Names)
* **Entities** that encapsulate identity and lifecycle
* **Value Objects** that encapsulate validation and invariants
* A clear separation between **transport concerns** (HTTP, JSON, DTOs) and **domain concerns** (business rules and state transitions)

The guiding rule throughout the codebase is:

> **Invalid domain state should be unrepresentable.**

* * *

## API boundary and DTOs

The HTTP boundary uses simple request/response DTOs composed of primitive types (`string`, `int`, `bool`, etc.). DTOs are treated strictly as **transport shapes**, not domain models.

* The **API layer** handles input validation, mapping, and HTTP concerns
* The **domain layer** enforces business invariants and rules

This separation prevents domain concepts from leaking across process boundaries and keeps the domain model independent of serialization and framework concerns.

* * *

## Domain model and invariants

The domain uses **Entities** and **Value Objects** to model the problem space:

* **Value Objects** (e.g., `Name`) are immutable and can only be created through factory methods that validate invariants such as length, whitespace normalization, and required constraints.

### Value object creation pattern

Value objects follow a consistent, defensive creation pattern to ensure invalid domain state is unrepresentable:

1. **Guard clauses**  null, empty, or required checks  
2. **Normalization**  trimming, casing, canonicalization  
3. **Invariant validation**  length, ranges, regex, business rules  
4. **Creation**  construct the immutable value object

This ordering avoids exceptions, keeps failures explicit, and ensures all value objects are created in a valid, normalized state.

  
* **Entities** (e.g., `Dish`, `Category`) encapsulate identity and lifecycle, and expose intention-revealing mutation methods (e.g., `Rename`, `ChangeCategory`) rather than public setters.
  

Domain constructors are kept private. Creation and mutation flow through explicit methods that return success/failure results, ensuring invariants are consistently enforced.

* * *

## Error handling and explicit results

The domain uses a `Result` / `Result<T>` style (via **CSharpFunctionalExtensions**) to represent business validation failures without exceptions.

The API layer translates domain results into consistent HTTP responses:

* `400`  invalid input or failed invariants
* `404`  missing resources
* `409`  conflicts (e.g., duplicate dish names within a category)

This approach keeps:

* domain logic deterministic and testable
* API behavior predictable
* failures explicit and easy to reason about

* * *

## Architecture: vertical slices over layers

The API is organized using a **vertical slice architecture** (feature folders). Each use case (e.g., Create Dish, Update Dish) has its endpoint, request/response contracts, validation, and mapping co-located.

This reduces cross-feature coupling and keeps the codebase easy to navigate as features grow.

* * *

## Persistence as an implementation detail

Entity Framework Core is used for persistence, but the domain model is not shaped around the ORM.

Where necessary (e.g., EF materialization), the domain includes minimal accommodations such as protected parameterless constructors, while preserving encapsulation and invariants.

* * *

## Solution layout

* **DinnerSpinner.Api**  ASP.NET Core 10 Web API built with **FastEndpoints**, using request/response DTOs and FluentValidation
* **DinnerSpinner.Client**  Blazor WebAssembly front end (Razor components) that consumes the API

The API and client are developed and run independently to reinforce separation of concerns.

* * *

## Features

* CRUD operations for dishes (name + category, such as *Eggs* / Breakfast, *BLT* / Lunch, *Spaghetti* / Dinner)
* Random “Spin” to select a meal
* Build a multi-day meal plan by spinning repeatedly
* SQLite persistence via Entity Framework Core

* * *

## Planned features

* Side dishes
* Favorites and weighted spins
* Weekly planner view
* Export / print meal plans and shopping lists
* Progressive Web App (offline support, installable)
* Family membership and live voting (WebSockets)

* * *

## Tech stack

* **.NET 10**
* **FastEndpoints**
* **FluentValidation**
* **Entity Framework Core**
* **SQLite**
* **Blazor WebAssembly**

* * *

## Getting started

### Prerequisites

* .NET 10 SDK

### 1) Run the API

    git clone https://github.com/DavidEaton/DinnerSpinner.git
    cd DinnerSpinner/DinnerSpinner.Api
    dotnet restore
    dotnet run

Notes:

* The API starts on a local port shown in the console
* Swagger UI is enabled for discovery and testing
* SQLite is used for local persistence
* The database file is created automatically on first run

### 2) Run the Blazor WebAssembly client

    cd ../DinnerSpinner.Client
    dotnet restore
    dotnet run

If the client and API run on different ports during development:

* Configure the API base URL in the client (e.g., `Program.cs`)
* Enable CORS in the API as needed

* * *

## API overview

The API follows a REST-style design and is discoverable via Swagger.

Typical endpoints include:

* `GET /dishes`  list all dishes
* `POST /dishes`  create a dish
* `GET /dishes/{id}`  retrieve a single dish
* `PUT /dishes/{id}`  update a dish
* `DELETE /dishes/{id}`  delete a dish

### API design notes

* Endpoints follow the **REPR** pattern (Request → Endpoint → Response)
* Validation is handled via **FluentValidation**
* Conflicts (e.g., duplicate dish names within a category) are explicitly detected
* Responses use a consistent envelope to simplify client handling

The “spin” operation currently happens client-side by requesting dishes and selecting a random item. This may later move server-side as rules grow more complex.

* * *

## Development notes

* Endpoints are implemented as small, focused vertical slices
* Business rules live in domain entities and value objects
* Entities and value objects represent the solution’s domain model
* The codebase favors readability and explicit behavior over clever abstractions

* * *

## Roadmap

* Side dishes
* Weighted spins (bias toward favorites)
* 7-day planner view
* PWA support (offline + install)
* Exportable shopping lists
* Household accounts
* Live voting

* * *

## Why this project exists

DinnerSpinner exists both as a useful household tool and as a practical demonstration of how I design, structure, and grow a backend-driven application. It reflects how I approach production systems: start small, design intentionally, and leave room for change.