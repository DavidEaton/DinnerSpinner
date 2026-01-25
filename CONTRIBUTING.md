Commit Message Conventions
--------------------------

This project uses **full commit messages** consisting of a **title** and an optional **body**.

### Commit title (required)

* One clear idea per commit

* Written in the **imperative mood** (“Introduce”, “Fix”, “Remove”)

* Describes _what the change does_, not how

* No trailing period

* Prefer ≤ ~72 characters

**Examples**

`Introduce explicit Dish contract and compose dish endpoint responsesFix EF relationship update in dishes update endpointRemove obsolete dish read response contracts`

### Commit body (required for non-trivial changes)

A commit body **must** be included when the change:

* Alters API contracts

* Introduces or changes architectural patterns

* Makes non-obvious design decisions

The body should:

* Explain **why** the change was made

* Provide context not immediately obvious from the diff

* Be wrapped at ~72 characters per line

**Example**

`Introduce a shared Dish contract and update create, update, and read endpoints to compose their response payloads around it.This makes endpoint response contracts explicit while avoidingduplication of dish fields across features.`

* * *

Naming Rules for Contracts and DTOs
-----------------------------------

This project favors **explicit, intention-revealing names** over generic or abbreviated ones.

### Request and Response types

* Every endpoint has its **own** `Request` and `Response` types

* These types live in the endpoint’s namespace

* Even if shapes are identical, they are **not shared** across endpoints

This makes endpoint contracts explicit and prevents accidental coupling.

### Shared API representations

When multiple endpoints need to expose the same representation of a domain concept:

* Use a shared **Contract** type

* Place it in the feature root namespace

* Name it simply `Contract` (not `DTO`, not `RequestResponse`)

**Example**

`DinnerSpinner.Api.Features.Dishes.Contract`

The feature namespace already conveys the domain context; repeating it in  
the type name is unnecessary.

### Why “Contract” instead of “DTO”

* “DTO” describes a mechanism

* “Contract” describes a promise

Contracts represent the **stable shape** the API exposes to clients.

* * *

Endpoint Contract Philosophy
----------------------------

Each endpoint in this project owns its API contract explicitly.

### Core principles

* Endpoints do **not** share request or response types

* Contracts are treated as **closed shapes**, not extension points

* Changes to contracts are intentional and explicit

### Composition over duplication

When multiple endpoints return the same data shape:

* Endpoint responses **compose** a shared `Contract`

* Endpoint-level `Response` types still exist

* The contract represents _what_ is returned; the response represents _why_

**Example**

`public sealed class Response {    public Contract Dish { get; init; } = new();}`

This preserves clarity while avoiding repeated field definitions.

### Identity and display values

API responses should return:

* Stable identifiers (`Id`, `CategoryId`)

* Human-readable values (`Name`, `CategoryName`)

Clients should not be forced to make additional calls simply to render UI  
or maintain normalized state.

### Requests vs responses

* **Requests** use identifiers for relationships (e.g. `CategoryId`)

* **Responses** include both identifiers and display values

* Nested domain entities are not accepted in requests
