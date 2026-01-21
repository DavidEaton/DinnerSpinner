# DinnerSpinner

DinnerSpinner is a small, end-to-end meal-planning application built with **ASP.NET Core** and **Blazor WebAssembly**.It helps households keep a list of home-cooked dishes and randomly “spin” to decide what to eat for the next meal or several days.

While intentionally modest in scope, this project is designed as a **portfolio showcase** demonstrating clean API design, vertical slice architecture, and pragmatic decision-making around evolving features.

* * *

## Goals & design focus

This project emphasizes **clarity and correctness over scale**. The primary goals are to:

* Practice **vertical slice architecture** using FastEndpoints (REPR pattern)
  
* Keep API endpoints cohesive, explicit, and easy to reason about
  
* Apply structured validation and consistent error handling
  
* Cleanly separate API and client responsibilities
  
* Favor simple persistence and incremental evolution over premature abstraction
  

DinnerSpinner reflects how I approach real-world systems: small, deliberate steps with room to grow.

* * *

## Solution layout

* **DinnerSpinner.Api**ASP.NET Core 10 Web API built with **FastEndpoints**, using request/response DTOs and FluentValidation.
  
* **DinnerSpinner.Client****Blazor WebAssembly** front end (Razor components) that consumes the API.
  

The API and client are developed and run independently to reinforce separation of concerns.

* * *

## Features

* CRUD operations for dishes (**name** + *category* such as ***Eggs** Breakfast, **BLT** Lunch, **Spaghetti** Dinner*)
  
* Random “Spin” to select a meal
  
* Build a multi-day meal plan by spinning repeatedly
  
* SQLite persistence via Entity Framework Core
  

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

* * *

### 1) Run the API

`git clone https://github.com/DavidEaton/DinnerSpinner.git cd DinnerSpinner/DinnerSpinner.Apidotnet restoredotnet run`

Notes:

* The API starts on a local port shown in the console.
  
* Swagger UI is enabled for discovery and testing.
  
* SQLite is used for local persistence.
  
* The database file is created automatically on first run.
  

* * *

### 2) Run the Blazor WebAssembly client

`cd ../DinnerSpinner.Clientdotnet restoredotnet run`

If the client and API run on different ports during development:

* Configure the API base URL in the client (e.g., `Program.cs`)
  
* Enable CORS in the API as needed
  

* * *

## API overview

The API follows a REST-style design, discoverable via Swagger.

Typical endpoints include:

* `GET /dishes` — list all dishes
  
* `POST /dishes` — create a dish
  
* `GET /dishes/{id}` — retrieve a single dish
  
* `PUT /dishes/{id}` — update a dish
  
* `DELETE /dishes/{id}` — delete a dish
  

### API design notes

* Endpoints follow the **REPR** pattern (Request → Endpoint → Response)
  
* Validation is handled via **FluentValidation**
  
* Conflicts (e.g., duplicate dish names within a category) are explicitly detected
  
* Responses use a consistent envelope to simplify client handling
  

The “spin” operation currently happens client-side by requesting dishes and selecting a random item. This may later move server-side as rules grow more complex.

* * *

### Development notes

* Endpoints are implemented as small, focused vertical slices
  
* Business rules live close to their endpoints
  
* Entities are intentionally minimal and expected to evolve
  
* The codebase favors readability and explicit behavior over clever abstractions
  

* * *

### Roadmap

* Side dishes
  
* Weighted spins (bias toward favorites)
  
* 7-day planner view
  
* PWA support (offline + install)
  
* Exportable shopping lists
  
* Family accounts and live meal voting
  

* * *

## Why this project exists

DinnerSpinner exists both as a useful household tool and as a practical demonstration of how I design, structure, and grow a backend-driven application. It reflects how I approach production systems: start small, design intentionally, and leave room for change.