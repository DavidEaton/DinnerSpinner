# DinnerSpinner

A tiny family app to help plan meals. Keep a list of dishes you can make at home and “spin” to randomly pick what to eat for the next few days (or the week). The project also serves as a hands-on way to get familiar with **FastEndpoints**.

## Solution layout

- **DinnerSpinner.Api** — ASP.NET Core 10 Web API built with **FastEndpoints**.
- **DinnerSpinner.Client** — **Blazor WebAssembly** front end (Razor components) that calls the API.

## Features

- CRUD for dishes aka courses (name, category i.e. BLT, Lunch).
- Random “Spin” to pick tonight’s meal.
- Build a quick multi-day plan by spinning repeatedly.
- **Planned:** Sides, favorites/weights (bias the spinner), weekly planner, export/print, and PWA install/offline.

## Tech stack

- **.NET 10**, **FastEndpoints**, **FluentValidation**
- **SQLite** (via Entity Framework Core) data persistence
- **Blazor WebAssembly** UI

## Getting started

### Prereqs
- .NET 10 SDK

### 1) Run the API

```bash
git clone https://github.com/DavidEaton/DinnerSpinner.git
cd DinnerSpinner/DinnerSpinner.Api
dotnet restore
dotnet run
````

* The API will start on a local port (shown in the console). Swagger UI is enabled for easy testing.
* A local SQLite DB is used (connection string in `appsettings*.json`). The DB file is created on first run if it doesn’t exist.

### 2) Run the Blazor WASM client

```bash
cd ../DinnerSpinner.Client
dotnet restore
dotnet run
```

> If the client calls the API on a different port, set the API base URL in the client (e.g., a config/appsettings or a constant in `Program.cs`). Enable CORS in the API if you’re serving them on different origins during dev.

## API (high level)

Typical REST-ish endpoints (discover via Swagger):

* `GET /dishes` — list all dishes
* `POST /dishes` — add a dish
* `GET /dishes/{id}` — fetch one
* `PUT /dishes/{id}` — update
* `DELETE /dishes/{id}` — remove

A client-side “spin” gets the list and picks a random item. 

## Development notes

* Endpoints follow **REPR** (Request → Endpoint → Response) for small, testable vertical slices.
* Validate inputs with **FluentValidation**.
* Keep entities minimal now; add `IsFavorite`, `Weight`, `Notes`, etc., as features grow.

## Roadmap

* Sides
* Weighted spins (favor favorites)
* 7-day planner view
* PWA: offline + “Add to Home Screen”
* Export/print a shopping list from the plan
* Family membership; live voting (websockets)