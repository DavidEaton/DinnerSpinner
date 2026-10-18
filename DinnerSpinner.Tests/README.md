# Domain unit tests

Run with the .NET 10 SDK from the repository root:

```sh
dotnet test DinnerSpinner.Tests/DinnerSpinner.Tests.csproj --configuration Release
```

The xUnit tests use Shouldly assertions and reference only the domain project.
They require no database, API host, or external services.

Coverage includes factory validation and normalization, boundary values, value
object equality, entity identity, intentional state changes, no-op changes,
rejected changes preserving state, and constructor/property encapsulation.
Reflection is used only to inspect public API visibility. Protected constructors
and identity setters remain available for persistence; test subclasses exercise
identity equality through that supported access. The tests do not manufacture
invalid value objects through reflection or persistence materialization.

`Dish` must reject a null category both during creation and category changes.
Those regression cases ensure a rejected update leaves the existing state intact.
