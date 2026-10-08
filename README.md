# Kejia-Cao---Paint-Management-System
A C# console application for managing paint products and orders. Homework in DistinctionCoding. 

## Project structure

- `Models/`: paint products, specifications, brands, stores, orders, users and payments.
- `Interfaces/`: contracts such as `IBuyable`.
- `Enums/`: paint types, payment statuses and payment methods.
- `Program.cs`: console demonstration of products, orders and user payment history.
- `Tests/PaintSystem.Tests/`: independent xUnit tests using assertions.

All application types retain the `PaintSystem` namespace. Internal types are exposed
only to the test assembly through `InternalsVisibleTo`.

## Run the demonstration

Requires the .NET 10 SDK. From the repository root:

```bash
dotnet run --project PaintSystem.csproj
```

The console output demonstrates application behavior; it is not the automated test report.

## Build and run automated tests

```bash
dotnet build PaintSystem.sln
dotnet test PaintSystem.sln
```

The test project references the application project. Tests cover the checks previously
mixed into `Program.cs`, plus Order, Payment and User queries, price boundaries,
product/quantity mapping and empty histories. NuGet packages are restored on the first run.

The project layout and test commands follow the
[Microsoft xUnit tutorial](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-xunit).
