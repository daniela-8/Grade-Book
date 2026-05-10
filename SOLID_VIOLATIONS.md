# SOLID Violations Report

This document describes each SOLID principle violation found in the original codebase, along with the applied fix.

---

## 1. Single Responsibility Principle (SRP)

**File:** `Controllers/ItemController.cs`, methods `GetAll()` and `GetById()`

**Why it is a violation:**
The controller is doing too many things at once. It handles HTTP request routing, but it also computes business statistics (total count, average value), performs manual logging through `Console.WriteLine`, and formats aggregated response objects. A controller should only be responsible for receiving requests and returning responses. Anything beyond that should be delegated elsewhere.

**Fix applied:**
All business logic was moved to a new `GradeService` class. The logging via `Console.WriteLine` was removed entirely (in a real production setup, the built-in ASP.NET Core logging middleware handles this). The controller now only delegates to `IGradeService` and returns the result.

---

## 2. Open/Closed Principle (OCP)

**File:** `Repositories/ItemRepository.cs`, method `GetAllAsync()`

**Why it is a violation:**
The filtering logic (`i.IsActive`) is hardcoded directly inside the repository method. If we wanted to change the filtering criteria or add new ones (for example, filtering by passing grade), we would need to modify the repository class itself. The class is not open for extension without modifying existing code.

**Fix applied:**
The repository (`GradeRepository`) now returns all data from the external source without applying any business filter. Filtering logic lives in the service layer (`GradeService.GetTopPassingGradesAsync`), which can be extended or composed without touching the data access code.

---

## 3. Liskov Substitution Principle (LSP)

**File:** `Repositories/ItemRepository.cs`

**Why it is a violation:**
The repository fields (`_items`, `_nextId`) and methods are declared as `protected virtual`, which signals that the class is designed for inheritance. Subclasses could override `GetAllAsync` or `GetByIdAsync` and change the behavioral contract (for example, returning inactive items or applying different ordering), which would break any code relying on the base class behavior.

**Fix applied:**
The new `GradeRepository` is a sealed-behavior implementation of `IGradeRepository`. There are no `virtual` or `protected` members. Extension is done by implementing the `IGradeRepository` interface rather than by inheriting from a concrete class.

---

## 4. Interface Segregation Principle (ISP)

**File:** `Interfaces/IItemReader.cs`

**Why it is a violation:**
The interface name is `IItemReader`, but the naming does not match the domain at all (the project is called GradeBook, not ItemBook). Beyond that, the system uses a single interface for the data access layer and there is no separate abstraction for the service layer. If a consumer only needs the filtered business operation (e.g., top passing grades), it still has to depend on the full reader interface.

**Fix applied:**
The interface was split into two distinct contracts: `IGradeRepository` for pure data access operations, and `IGradeService` for business-level operations. Each consumer depends only on the interface it actually needs. The controller depends on `IGradeService`, the service depends on `IGradeRepository`.

---

## 5. Dependency Inversion Principle (DIP)

**File:** `Program.cs`

**Why it is a violation:**
The original `Program.cs` registers `AddControllers()` but never registers the `IItemReader` implementation in the dependency injection container. This means the application would crash at runtime when the controller tries to resolve `IItemReader`. The high-level module (controller) depends on an abstraction, but the abstraction was never wired to a concrete implementation.

**Fix applied:**
The new `Program.cs` properly registers all dependencies:
- `IGradeRepository` -> `GradeRepository` (via `AddHttpClient` for typed HTTP client support)
- `IGradeService` -> `GradeService` (via `AddScoped`)

Every class depends on an interface, and the DI container wires the concrete implementations at application startup.

---

## Additional issues (not strictly SOLID, but relevant)

### Domain naming mismatch

The project is called `Siemens.Internship2026.GradeBook`, but all classes were named around a generic `Item` concept. This makes the code confusing and disconnected from its purpose. All types were renamed to use `Grade` terminology (`Grade`, `GradeController`, `GradeService`, `GradeRepository`, etc.).

### In-memory data source

The original `ItemRepository` used a hardcoded in-memory list that was always empty (no items were ever added to `_items`). The repository was refactored to fetch data from the external JSON endpoint as required by the assignment.
