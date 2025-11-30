# Copilot Instructions for Proyecto_Moviles_Backend

## Project Overview
A C# ASP.NET Core 8.0 REST API backend for a mobile HR platform. The system manages profiles, vacancies, skills, and related entities with a Supabase PostgreSQL database backend.

## Architecture Overview

### Core Pattern: Generic Repository via Supabase Service
All data access flows through `Services/SupabaseService.cs`, a singleton that wraps the Supabase client and provides generic CRUD methods:
- `GetAllAsync<T>(table)` - fetch all records
- `GetByIdAsync<T>(table, keyColumn, keyValue)` - fetch by primary key
- `CreateAsync<T>(table, item)` - insert and return created record
- `UpdateAsync<T>(table, keyColumn, keyValue, item)` - PATCH and return updated record
- `DeleteAsync(table, keyColumn, keyValue)` - delete by key

**Key Detail**: The service constructs REST calls to Supabase's PostgREST API (`/rest/v1/{table}`), not the Supabase C# SDK for data operations. JSON serialization uses `PropertyNameCaseInsensitive` to map C# PascalCase to snake_case database columns.

### Controller Pattern
Every controller (e.g., `ProfilesController`, `SkillsController`, `VacanciesController`) follows the same structure:
1. Inject `SupabaseService` via constructor DI
2. Implement `GetAll`, `GetById`, `Create`, `Update`, `Delete` methods
3. Use `[Route("api/[controller]")]` for RESTful routes with ID routing parameters (`{id:guid}` or `{id:int}`)
4. Return appropriate status codes: `200 Ok`, `201 CreatedAtAction`, `204 NoContent`, `404 NotFound`

**Example ID convention**: Profiles use `Guid` (`[HttpGet("{id:guid}")]`), Skills/Vacancies use `int` (`[HttpGet("{id:int}")]`).

### Database Models
All models in `Models/` use `[JsonPropertyName]` attributes to map C# properties to snake_case database columns:
```csharp
[JsonPropertyName("full_name")]
public string FullName { get; set; }
```

Key entity relationships:
- **Profile** → owns many ProfileSkill records (via department context)
- **Vacancy** → owns many VacancySkill records (required skills for a position)
- **Skill** → referenced by both Profile and Vacancy through junction tables
- **Department** → referenced by both Profile and Vacancy

## Critical Workflows

### Building & Running
```powershell
# Build
dotnet build

# Run (development with Swagger)
dotnet run

# Run specific environment
dotnet run --configuration Debug
```

Swagger UI auto-enables in Development environment (see `Program.cs`).

### Adding a New Entity Endpoint
1. Create model in `Models/YourEntity.cs` with `[JsonPropertyName]` attributes
2. Create controller in `Controllers/YourEntityController.cs` inheriting `ControllerBase`
3. Inject `SupabaseService` and implement CRUD methods using appropriate generic calls
4. Add controller registration (automatic via `AddControllers()` in `Program.cs`)

### Configuration
- **Supabase credentials** live in `appsettings.json` (Url, Key) and are injected via `IConfiguration`
- **Environment-specific overrides** use `appsettings.Development.json` (logging only currently)
- **CORS policy** is wide-open for Android development (`AllowAnyOrigin/Header/Method`)

## Project-Specific Patterns & Conventions

### Database Column Naming
- Database uses **snake_case**: `full_name`, `is_available_for_change`, `created_at`
- C# models use **PascalCase** with `[JsonPropertyName]` mapping
- Supabase service handles automatic camel-case-insensitive deserialization

### Asynchronous All The Way
All service methods are `async Task<T>` and must be `await`ed. Controllers use `async Task<IActionResult>` pattern.

### Error Handling
- Service calls use `resp.EnsureSuccessStatusCode()` to throw on HTTP errors
- Controllers check for `null` returns and respond with `NotFound()` appropriately
- No custom exception handling currently in place; consider adding middleware for API-wide error responses

### ID Types by Entity
- **Profiles**: `Guid` (natural fit for user identifiers)
- **Skills, Vacancies, Departments**: `int` (auto-increment foreign keys)
- Always validate route constraints match model properties

## Key Files Reference
| File | Purpose |
|------|---------|
| `Program.cs` | App configuration, DI registration, middleware setup |
| `Services/SupabaseService.cs` | Database abstraction layer; all data access flows here |
| `Controllers/*.cs` | REST endpoints; follow the pattern established in ProfilesController |
| `Models/*.cs` | Data transfer objects with snake_case JSON mapping |
| `appsettings.json` | Supabase URL and API key configuration |

## Integration Points
- **Supabase Client**: Initialized in service constructor with `AutoConnectRealtime: true`
- **PostgREST API**: Used for all CRUD operations via HttpClient to `/rest/v1/{table}`
- **Android App**: CORS allows requests from any origin (development only)

## Testing & Validation
- Use `Api.http` file for manual endpoint testing (likely REST client format)
- Swagger available at `/swagger/ui` in Development
- No unit test project currently; consider `xUnit` or `NUnit` for future coverage
